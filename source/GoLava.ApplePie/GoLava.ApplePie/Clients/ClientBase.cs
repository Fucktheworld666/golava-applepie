using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Contracts.Attributes;
using GoLava.ApplePie.Exceptions;
using GoLava.ApplePie.Formatting;
using GoLava.ApplePie.Threading;
using GoLava.ApplePie.Transfer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoLava.ApplePie.Clients
{
    public class ClientBase<TUrlProvider>
        where TUrlProvider: IUrlProvider
    {
        private readonly NamedFormatter _namedFormatter;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        protected ClientBase(TUrlProvider urlProvider)
            : this(new RestClient(), urlProvider) { }

        protected ClientBase(RestClient restClient, TUrlProvider urlProvider)
        {
            _namedFormatter = new NamedFormatter();
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            this.RestClient = restClient;
            this.UrlProvider = urlProvider;
        }

        protected RestClient RestClient { get; }

        protected TUrlProvider UrlProvider { get; }

        public async Task<ClientContext> LogonAsync(string username, string password)
        {
            await Configure.AwaitFalse();

            var context = new ClientContext();
            try
            {
                var logon = await this.LogonAsync(context, new Credentials
                {
                    AccountName = username,
                    Password = password
                });

                var session = await this.GetSessionAsync(context);
                context.Authentication = Authentication.Success;
                context.Session = session;
            }
            catch (ApplePieException)
            {
                // todo log exception
                context.Authentication = Authentication.Failed;
            }
            return context;
        }

        protected Uri CreateUri(string url, object urlArguments = null)
        {
            if (urlArguments != null)
                url = _namedFormatter.Format(url, urlArguments);
            return new Uri(url);
        }

        protected virtual async Task<Logon> LogonAsync(ClientContext context, Credentials credentials)
        {
            await Configure.AwaitFalse();

            try
            {
                if (context.AuthToken == null)
                    context.AuthToken = await this.GetAuthTokenAsync(context);

                var headers = new RestHeaders
                {
                    { "X-Apple-Widget-Key", context.AuthToken.AuthServiceKey }
                };
                var request = RestRequest.Post(
                    new RestUri(this.UrlProvider.LogonUrl), headers, RestContentType.Json, credentials);

                var response = await this.SendAsync<Logon>(context, request);
                return response.Content;
            }
            catch (ApplePieException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplePieException("Failed to logon.", ex);
            }
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
                throw new ApplePieException("Failed to get session.", ex);
            }
        }

        protected async Task<RestResponse<TContent>> SendAsync<TContent>(ClientContext context, RestRequest request)
        {
            await Configure.AwaitFalse();

            if (request.Headers == null)
                request.Headers = new RestHeaders();
            request.Headers.Add("Accept", "application/json", "text/javascript");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            var csrfClass = FindCsrfClass(typeof(TContent));
            if (context.TryGetValue(out CsrfToken csrfToken, csrfClass))
            {
                request.Headers.Add("csrf", csrfToken.Value);
                request.Headers.Add("csrf_ts", csrfToken.Timestamp);
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

            switch (response.ContentType)
            {
                case RestContentType.Json:
                    var error = JsonConvert.DeserializeObject<Error>(response.RawContent, _jsonSerializerSettings);
                    if (error.ServiceErrors != null && error.ServiceErrors.Count > 0)
                    {
                        var serviceMessage = error.ServiceErrors.First();
                        message = $"{serviceMessage.Message} ({serviceMessage.Code})";
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

            throw new ApplePieException(message);
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
                throw new ApplePieException("Failed to get auth token.", ex);
            }
        }

        private CsrfToken GetCsrfToken<TContent>(RestResponse<TContent> response, CsrfClass csrfClass)
        {
            if (!response.Headers.TryGetValue("csrf", out HashSet<string> csrfValues) || csrfValues.Count != 1)
                return null;
            if (!response.Headers.TryGetValue("csrf_ts", out HashSet<string> csrfTsValues) || csrfTsValues.Count != 1)
                return null;

            var csrfToken = new CsrfToken
            {
                Timestamp = csrfTsValues.First(),
                Value = csrfValues.First(),
                Class = csrfClass
            };
            return csrfToken;
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
