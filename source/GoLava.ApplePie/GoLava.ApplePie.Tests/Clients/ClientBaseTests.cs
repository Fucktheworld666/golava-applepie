using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GoLava.ApplePie.Clients;
using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Serializers;
using GoLava.ApplePie.Transfer;
using RichardSzalay.MockHttp;
using Xunit;

namespace GoLava.ApplePie.Tests.Clients
{
    public abstract class ClientBaseTests<TUrlProvider>
        where TUrlProvider : IUrlProvider
    {
        private const string Expected_AuthToken_AuthServiceKey = "auth-service-key";
        private const string Expected_AuthToken_AuthServiceUrl = "auth-service-url";

        private const string Expected_Session_User_Id = "id";
        private const string Expected_Session_User_EmailAddress = "email-address";
        private const string Expected_Session_User_FullName = "full-name";

        private const string Expected_TwoStep_SessionId = "session-id";
        private const string Expected_TwoStep_Scnt = "scnt";

        private const string Expected_TrustedDevice_Id = "id";

        private const string Expected_SecurityCode = "1234";

        protected ClientBaseTests(TUrlProvider urlProvider)
        {
            this.UrlProvider = urlProvider;
        }

        protected TUrlProvider UrlProvider { get; }

        protected JsonSerializer JsonSerializer { get; } = new JsonSerializer();

        [Fact]
        public async Task LogonWithInvalidCredentialsSetsAuthenticationToFailed()
        {
            var context = await this.LogonWithInvalidCredentialsAsync("foo", "bar");
            Assert.Equal(Authentication.FailedWithInvalidCredentials, context.Authentication);
        }

        [Fact]
        public async Task LogonWithInvalidCredentialsReadsAuthToken()
        {
            var context = await this.LogonWithInvalidCredentialsAsync("foo", "bar");
            Assert.Equal(Expected_AuthToken_AuthServiceKey, context.AuthToken.AuthServiceKey);
            Assert.Equal(Expected_AuthToken_AuthServiceUrl, context.AuthToken.AuthServiceUrl);
        }

        [Fact]
        public async Task LogonWithValidCredentialsSetsAuthenticationToSuccess()
        {
            var context = await this.LogonWithValidCredentialsAsync("foo", "bar");
            Assert.Equal(Authentication.Success, context.Authentication);
        }

        [Fact]
        public async Task LogonWithValidCredentialsReadsAuthToken()
        {
            var context = await this.LogonWithValidCredentialsAsync("foo", "bar");
            Assert.Equal(Expected_AuthToken_AuthServiceKey, context.AuthToken.AuthServiceKey);
            Assert.Equal(Expected_AuthToken_AuthServiceUrl, context.AuthToken.AuthServiceUrl);
        }

        [Fact]
        public async Task LogonWithValidCredentialsReadsSessionUser()
        {
            var context = await this.LogonWithValidCredentialsAsync("foo", "bar");
            Assert.Equal(Expected_Session_User_Id, context.Session.User.Id);
            Assert.Equal(Expected_Session_User_EmailAddress, context.Session.User.EmailAddress);
            Assert.Equal(Expected_Session_User_FullName, context.Session.User.FullName);
        }

        [Fact]
        public async Task LogonWithValidCredentialsAndTwoStepSetsAuthenticationToTwoStepSelectTrustedDevice()
        {
            var context = await this.LogonWithValidCredentialsTwoStepAsync("foo", "bar");
            Assert.Equal(Authentication.TwoStepSelectTrustedDevice, context.Authentication);
        }

        [Fact]
        public async Task AcquireTwoStepCodeAsyncSetsAuthenticationToTwoStepCode()
        {
            var context = await this.AcquireTwoStepCodeAsync(new TrustedDevice {
                Id = Expected_TrustedDevice_Id
            });
            Assert.Equal(Authentication.TwoStepCode, context.Authentication);
        }

        [Fact]
        public async Task LoginWithTwoStepCodeAsyncSetsAuthenticationToSuccess()
        {
            var context = await this.LoginWithTwoStepCodeAsync(new TrustedDevice
            {
                Id = Expected_TrustedDevice_Id
            }, Expected_SecurityCode);
            Assert.Equal(Authentication.Success, context.Authentication);
        }

        protected async Task<ClientContext> AcquireTwoStepCodeAsync(TrustedDevice trustedDevice)
        {
            var context = new ClientContext
            {
                AuthToken = new AuthToken
                {
                    AuthServiceKey = Expected_AuthToken_AuthServiceKey,
                    AuthServiceUrl = Expected_AuthToken_AuthServiceUrl
                },
                TwoStepToken = new TwoStepToken
                {
                    Scnt = Expected_TwoStep_Scnt,
                    SessionId = Expected_TwoStep_SessionId
                }
            };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp = this.AddSecurityCodeExpectation(mockHttp, trustedDevice, null);

            var client = this.CreateClient(new RestClient(mockHttp), this.UrlProvider);
            context = await client.AcquireTwoStepCodeAsync(context, trustedDevice);

            mockHttp.VerifyNoOutstandingExpectation();

            return context;
        }

        protected async Task<ClientContext> LogonWithInvalidCredentialsAsync(string username, string password)
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp = this.AddAuthTokenExpectation(mockHttp);
            mockHttp = this.AddLogonExpectation(mockHttp, username, password, HttpStatusCode.Forbidden, null);

            var client = this.CreateClient(new RestClient(mockHttp), this.UrlProvider);
            var context = await client.LogonWithCredentialsAsync(username, password);

            mockHttp.VerifyNoOutstandingExpectation();

            return context;
        }

        protected async Task<ClientContext> LoginWithTwoStepCodeAsync(TrustedDevice trustedDevice, string code)
        {
            var context = new ClientContext
            {
                AuthToken = new AuthToken
                {
                    AuthServiceKey = Expected_AuthToken_AuthServiceKey,
                    AuthServiceUrl = Expected_AuthToken_AuthServiceUrl
                },
                TwoStepToken = new TwoStepToken
                {
                    Scnt = Expected_TwoStep_Scnt,
                    SessionId = Expected_TwoStep_SessionId
                }
            };
            context.AddValue(trustedDevice);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp = this.AddSecurityCodeExpectation(mockHttp, trustedDevice, code);

            var client = this.CreateClient(new RestClient(mockHttp), this.UrlProvider);
            context = await client.LogonWithTwoStepCodeAsync(context, code);

            mockHttp.VerifyNoOutstandingExpectation();

            return context;
        }

        protected async Task<ClientContext> LogonWithValidCredentialsAsync(string username, string password)
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp = this.AddAuthTokenExpectation(mockHttp);
            mockHttp = this.AddLogonExpectation(mockHttp, username, password, HttpStatusCode.OK, new LogonAuth 
            { 
                AuthType = "sa" 
            });

            var client = this.CreateClient(new RestClient(mockHttp), this.UrlProvider);
            var context = await client.LogonWithCredentialsAsync(username, password);

            mockHttp.VerifyNoOutstandingExpectation();

            return context;
        }

        protected async Task<ClientContext> LogonWithValidCredentialsTwoStepAsync(string username, string password)
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp = this.AddAuthTokenExpectation(mockHttp);
            mockHttp = this.AddLogonExpectation(mockHttp, username, password, HttpStatusCode.Conflict, 
                new LogonAuth 
                { 
                    AuthType = "hsa" 
                }, 
                new Dictionary<string, string> {
                    { "x-apple-id-session-id", Expected_TwoStep_SessionId },
                    { "scnt", Expected_TwoStep_Scnt }
                });

            var client = this.CreateClient(new RestClient(mockHttp), this.UrlProvider);
            var context = await client.LogonWithCredentialsAsync(username, password);

            mockHttp.VerifyNoOutstandingExpectation();

            return context;
        }

        protected abstract ClientBase<TUrlProvider> CreateClient(RestClient restClient, TUrlProvider urlProvider);

        protected MockHttpMessageHandler AddSecurityCodeExpectation(MockHttpMessageHandler mockHttp, TrustedDevice trustedDevice, string code)
        {
            var method = string.IsNullOrEmpty(code) ? HttpMethod.Put : HttpMethod.Post;
            var mockRequest = mockHttp
                .Expect(method, new RestUri(this.UrlProvider.TwoStepVerifyUrl, new {
                    deviceId = trustedDevice.Id
                }).AbsoluteUri)
                .WithHeaders(new[] {
                    new KeyValuePair<string, string>("Accept", "application/json"),
                    new KeyValuePair<string, string>("Accept", "text/javascript"),
                    new KeyValuePair<string, string>("X-Requested-With", "XMLHttpRequest"),
                    new KeyValuePair<string, string>("X-Apple-Widget-Key", Expected_AuthToken_AuthServiceKey),
                    new KeyValuePair<string, string>("x-apple-id-session-id", Expected_TwoStep_SessionId),
                    new KeyValuePair<string, string>("scnt", Expected_TwoStep_Scnt)
                })
                .Respond(HttpStatusCode.OK);
            if (method == HttpMethod.Post)
            {
                mockRequest.WithContent(this.JsonSerializer.Serialize(new { code }));

                mockHttp
                    .Expect(HttpMethod.Get, this.UrlProvider.SessionUrl)
                    .Respond(HttpStatusCode.OK, "application/json", this.JsonSerializer.Serialize(new Session
                    {
                        User = new User
                        {
                            EmailAddress = Expected_Session_User_EmailAddress,
                            FullName = Expected_Session_User_FullName,
                            Id = Expected_Session_User_Id
                        }
                    }));
            }

            return mockHttp;
        }

        protected MockHttpMessageHandler AddLogonExpectation(
            MockHttpMessageHandler mockHttp, 
            string username, string password, 
            HttpStatusCode statusCode, LogonAuth logonAuth = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var mockRequest = mockHttp
                .Expect(HttpMethod.Post, this.UrlProvider.LogonUrl)
                .WithContent(this.JsonSerializer.Serialize(new
                {
                    accountName = username,
                    password,
                    rememberMe = true
                }))
                .WithHeaders(new[] {
                    new KeyValuePair<string, string>("Accept", "application/json"),
                    new KeyValuePair<string, string>("Accept", "text/javascript"),
                    new KeyValuePair<string, string>("X-Requested-With", "XMLHttpRequest"),
                    new KeyValuePair<string, string>("X-Apple-Widget-Key", Expected_AuthToken_AuthServiceKey)
                });
            if (logonAuth != null)
            {
                // when logon auth is set, we need to return it as content
                if (headers != null)
                    mockRequest.Respond(statusCode, headers, "application/json", this.JsonSerializer.Serialize(logonAuth));
                else
                    mockRequest.Respond(statusCode, "application/json", this.JsonSerializer.Serialize(logonAuth));

                if (statusCode == HttpStatusCode.OK)
                {
                    // when status code is ok, we expect to acquire a session
                    mockHttp
                        .Expect(HttpMethod.Get, this.UrlProvider.SessionUrl)
                        .Respond(HttpStatusCode.OK, "application/json", this.JsonSerializer.Serialize(new Session
                        {
                            User = new User
                            {
                                EmailAddress = Expected_Session_User_EmailAddress,
                                FullName = Expected_Session_User_FullName,
                                Id = Expected_Session_User_Id
                            }
                        }));
                } 
                else if (statusCode == HttpStatusCode.Conflict)
                {
                    if (logonAuth.AuthType == "hsa")
                    {
                        // when status code is conflict and auth type is hsa, we expect to call two step authentication

                        mockHttp
                            .Expect(HttpMethod.Get, this.UrlProvider.TwoStepAuthUrl)
                            .WithHeaders(headers ?? Enumerable.Empty<KeyValuePair<string, string>>())
                            .Respond(HttpStatusCode.OK, "application/json", this.JsonSerializer.Serialize(logonAuth));
                    }
                }
            }
            else
            {
                mockRequest.Respond(statusCode);
            }

            return mockHttp;
        }

        protected MockHttpMessageHandler AddAuthTokenExpectation(MockHttpMessageHandler mockHttp)
        {
            mockHttp
                .Expect(HttpMethod.Get, this.UrlProvider.AuthTokenUrl)
                .WithHeaders(new[] {
                    new KeyValuePair<string, string>("Accept", "application/json"),
                    new KeyValuePair<string, string>("Accept", "text/javascript"),
                    new KeyValuePair<string, string>("X-Requested-With", "XMLHttpRequest")
                })
                .Respond(HttpStatusCode.OK, "application/json", this.JsonSerializer.Serialize(new AuthToken 
                {
                    AuthServiceKey = Expected_AuthToken_AuthServiceKey,
                    AuthServiceUrl = Expected_AuthToken_AuthServiceUrl
                }));
            return mockHttp;
        }
    }
}
