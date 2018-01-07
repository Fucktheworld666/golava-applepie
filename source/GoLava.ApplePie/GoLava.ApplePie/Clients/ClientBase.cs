using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Contracts.Attributes;
using GoLava.ApplePie.Exceptions;
using GoLava.ApplePie.Formatting;
using GoLava.ApplePie.Threading;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Clients
{
    public class ClientBase<TUrlProvider>
        where TUrlProvider: IUrlProvider
    {
        private readonly NamedFormatter _namedFormatter;

        protected ClientBase(TUrlProvider urlProvider)
            : this(new RestClient(), urlProvider) { }

        protected ClientBase(RestClient restClient, TUrlProvider urlProvider)
        {
            _namedFormatter = new NamedFormatter();

            this.RestClient = restClient;
            this.UrlProvider = urlProvider;
        }

        protected RestClient RestClient { get; }

        protected TUrlProvider UrlProvider { get; }

        public async Task<ClientContext> LogonWithCredentialsAsync(string username, string password)
        {
            await Configure.AwaitFalse();

            var context = new ClientContext();
            try
            {
                var logonAuth = await this.LogonWithCredentialsAsync(context, username, password);
                context.LogonAuth = logonAuth;
                if (!logonAuth.IsTwoStepRequired)
                {
                    var session = await this.GetSessionAsync(context);
                    context.Authentication = Authentication.Success;
                    context.Session = session;
                }
                else 
                {
                    context.Authentication = Authentication.TwoStepSelectTrustedDevice;    
                }
            }
            catch (ApplePieCredentialsException)
            {
                // todo log exception
                context.Authentication = Authentication.FailedWithInvalidCredentials;
            }
            catch (ApplePieException)
            {
                // todo log exception
                context.Authentication = Authentication.FailedUnexpected;
            }
            return context;
        }

        public async Task<ClientContext> AcquireTwoStepCodeAsync(ClientContext context, TrustedDevice trustedDevice)
        {
            await Configure.AwaitFalse();

            var logonAuth = await this.LogonWithTwoStepCodeAsync(context, trustedDevice, null);
            context.Authentication = Authentication.TwoStepCode;
            context.LogonAuth = logonAuth;
            context.AddValue(trustedDevice);
            return context;
        }

        public async Task<ClientContext> LogonWithTwoStepCodeAsync(ClientContext context, string code)
        {
            await Configure.AwaitFalse();

            try
            {
                if (context.TryGetValue(out TrustedDevice trustedDevice))
                {
                    var logonAuth = await this.LogonWithTwoStepCodeAsync(context, trustedDevice, code);
                    context.LogonAuth = logonAuth;

                    var session = await this.GetSessionAsync(context);
                    context.Authentication = Authentication.Success;
                    context.Session = session;

                    context.DeleteValue<TrustedDevice>();
                }
                else 
                {
                    var logonAuth = context.LogonAuth;
                    context.Authentication = logonAuth != null && logonAuth.TrustedDevices != null && logonAuth.TrustedDevices.Count > 0 
                        ? Authentication.TwoStepSelectTrustedDevice : Authentication.FailedNoTrustedDeviceFound;
                }
                return context;
            }
            catch (ApplePieRestException<LogonAuth> apre)
            {
                switch (apre.Response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        if (apre.ErrorCode == ErrorCode.IncorrectVerificationCode)
                        {
                            context.Authentication = Authentication.TwoStepCode;
                            return context;
                        }
                        break;
                }
                throw;
            }
            catch (ApplePieException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplePieException("Failed to verify two-step authentication. See inner exception for more details.", ex);
            }
        }

        protected Uri CreateUri(string url, object urlArguments = null)
        {
            if (urlArguments != null)
                url = _namedFormatter.Format(url, urlArguments);
            return new Uri(url);
        }

        protected virtual async Task<LogonAuth> LogonWithCredentialsAsync(ClientContext context, string username, string password)
        {
            await Configure.AwaitFalse();

            try
            {
                if (context.AuthToken == null)
                    context.AuthToken = await this.GetAuthTokenAsync(context);
                
                var request = RestRequest.Post(
                    new RestUri(this.UrlProvider.LogonUrl), 
                    this.GetAuthRequestHeaders(context), 
                    RestContentType.Json, new {
                        accountName = username,
                        password,
                        rememberMe = true
                    });

                var response = await this.SendAsync<LogonAuth>(context, request);
                return response.Content;
            }
            catch (ApplePieRestException<LogonAuth> apre)
            {
                switch (apre.Response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        throw new ApplePieCredentialsException($"Invalid username and password combination. Used '{username}' as the username.", apre);

                    case HttpStatusCode.Conflict:
                        return await this.HandleTwoStepAuthenticationAsync(context, apre.Response);
                }
                throw;
            }
            catch (ApplePieException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplePieException("Failed to logon. See inner exception for more details.", ex);
            }
        }

        protected virtual async Task<LogonAuth> LogonWithTwoStepCodeAsync(ClientContext context, TrustedDevice trustedDevice, string code)
        {
            await Configure.AwaitFalse();

            RestRequest request;
            if (string.IsNullOrEmpty(code))
            {
                request = RestRequest.Put(
                    new RestUri(this.UrlProvider.TwoStepVerifyUrl, new { deviceId = trustedDevice.Id }),
                    this.GetTwoStepHeaders(context));
            }
            else
            {
                request = RestRequest.Post(
                    new RestUri(this.UrlProvider.TwoStepVerifyUrl, new { deviceId = trustedDevice.Id }),
                    this.GetTwoStepHeaders(context),
                    RestContentType.Json,
                    new { code });
            }
            var response = await this.SendAsync<LogonAuth>(context, request);
            return response.Content;
        }


        protected async Task<Session> GetSessionAsync(ClientContext context)
        {
            await Configure.AwaitFalse();

            try
            {
                var request = RestRequest.Get(new RestUri(this.UrlProvider.SessionUrl));
                var response = await this.SendAsync<Session>(context, request);
                return response.Content;
            }
            catch (ApplePieException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplePieException("Failed to get session. See inner exception for more details.", ex);
            }
        }

        protected async Task<RestResponse<TContent>> SendAsync<TContent>(ClientContext context, RestRequest request)
        {
            await Configure.AwaitFalse();

            if (request.Headers == null)
                request.Headers = new RestHeaders();
            request.Headers.Set("Accept", "application/json", "text/javascript");
            request.Headers.Set("X-Requested-With", "XMLHttpRequest");

            var csrfClass = FindCsrfClass(typeof(TContent));
            if (context.TryGetValue(out CsrfToken csrfToken, csrfClass))
            {
                request.Headers.Set("csrf", csrfToken.Value);
                request.Headers.Set("csrf_ts", csrfToken.Timestamp);
            }

            var response = await this.RestClient.SendAsync<TContent>(context, request);
            if (response.IsSuccess)
            {
                csrfToken = this.GetCsrfToken(response, csrfClass);
                if (csrfToken != null)
                    context.AddValue(csrfToken, csrfToken.Class);
                return response;
            }

            // try to handle error
            var message = "Failed to send request.";

            var errorCode = ErrorCode.Unknown;
            switch (response.ContentType)
            {
                case RestContentType.Json:
                    var error = this.RestClient.Serializer.Deserialize<Error>(response.RawContent);
                    if (error.ServiceErrors != null && error.ServiceErrors.Count > 0)
                    {
                        var serviceError = error.ServiceErrors.First();
                        message = $"{serviceError.Message} ({serviceError.Code})";
                        errorCode = new ErrorCode(serviceError.Code);
                    }
                    else if (error.ValidationErrors != null && error.ValidationErrors.Count > 0)
                    {
                        var validationError = error.ValidationErrors.First();
                        message = $"{validationError.Message} ({validationError.Code})";
                        errorCode = new ErrorCode(validationError.Code);
                    }
                    else if (!string.IsNullOrEmpty(error.UserString))
                    {
                        message = error.UserString;
                    }
                    break;
                case RestContentType.Text:
                    message = response.RawContent;
                    break;
            }

            throw new ApplePieRestException<TContent>(message, response, errorCode);
        }

        private async Task<LogonAuth> HandleTwoStepAuthenticationAsync(
            ClientContext context, 
            RestResponse<LogonAuth> logonAuthResponse)
        {
            await Configure.AwaitFalse();

            var logonAuth = this.RestClient.Serializer.Deserialize<LogonAuth>(logonAuthResponse.RawContent);
            if (!logonAuth.AuthType.Equals("hsa", StringComparison.OrdinalIgnoreCase))
                throw new ApplePieException($"Unknown authentication type '{logonAuthResponse.Content.AuthType}'");

            context.TwoStepToken = this.GetTwoStepToken(logonAuthResponse);
            if (context.TwoStepToken == null)
                throw new ApplePieException("Failed to get two-step token.");

            var request = RestRequest.Get(
                new RestUri(this.UrlProvider.TwoStepAuthUrl), this.GetTwoStepHeaders(context));
            var response = await this.SendAsync<LogonAuth>(context, request);
            return response.Content;
        }

        private async Task<AuthToken> GetAuthTokenAsync(ClientContext context)
        {
            await Configure.AwaitFalse();

            try
            {
                var request = RestRequest.Get(new RestUri(this.UrlProvider.AuthTokenUrl));
                var response = await this.SendAsync<AuthToken>(context, request);

                var authToken = response.Content;
                if (authToken == null || string.IsNullOrEmpty(authToken.AuthServiceKey))
                    throw new ApplePieException("Auth token service key not set.");

                return authToken;
            }
            catch (ApplePieException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplePieException("Failed to get auth token. See inner exception for more details.", ex);
            }
        }

        private RestHeaders GetAuthRequestHeaders(ClientContext context)
        {
            if (context.AuthToken == null)
                throw new ApplePieException("No auth token set.");

            if (string.IsNullOrEmpty(context.AuthToken.AuthServiceKey))
                throw new ApplePieException("No auth token service key set.");

            var headers = new RestHeaders
            {
                { "X-Apple-Widget-Key", context.AuthToken.AuthServiceKey }
            };
            return headers;
        }

        private RestHeaders GetTwoStepHeaders(ClientContext context)
        {
            if (context.TwoStepToken == null)
                throw new ApplePieException("No two-step token set.");

            if (string.IsNullOrEmpty(context.TwoStepToken.SessionId))
                throw new ApplePieException("No two-step token session id set.");
            if (string.IsNullOrEmpty(context.TwoStepToken.Scnt))
                throw new ApplePieException("No two-step token scnt set.");

            var headers = this.GetAuthRequestHeaders(context);
            headers.Add("X-Apple-Id-Session-Id", context.TwoStepToken.SessionId);
            headers.Add("scnt", context.TwoStepToken.Scnt);
            return headers;
        }

        private CsrfToken GetCsrfToken<TContent>(RestResponse<TContent> response, CsrfClass csrfClass)
        {
            if (!response.Headers.TryGetValue("csrf", out string csrfValue))
                return null;
            if (!response.Headers.TryGetValue("csrf_ts", out string csrfTsValue))
                return null;

            var csrfToken = new CsrfToken
            {
                Timestamp = csrfTsValue,
                Value = csrfValue,
                Class = csrfClass
            };
            return csrfToken;
        }

        private TwoStepToken GetTwoStepToken<TContent>(RestResponse<TContent> response)
        {
            if (!response.Headers.TryGetValue("x-apple-id-session-id", out string sessionIdValue))
                return null;
            if (!response.Headers.TryGetValue("scnt", out string scntValue))
                return null;

            var twoStepToken = new TwoStepToken
            {
                Scnt = scntValue,
                SessionId = sessionIdValue
            };
            return twoStepToken;
        }

        private static CsrfClass FindCsrfClass(Type type)
        {
            var csrfClassAttribute = type.GetCustomAttribute<CsrfClassAttribute>();
            if (csrfClassAttribute != null)
                return csrfClassAttribute.CsrfClass;

            if (type.IsGenericType)
            {
                foreach (var genericType in type.GenericTypeArguments)
                {
                    var csrfClass = FindCsrfClass(genericType);
                    if (csrfClass != CsrfClass.Undefined)
                        return csrfClass;
                }
            }

            return CsrfClass.Undefined;
        }
    }
}
