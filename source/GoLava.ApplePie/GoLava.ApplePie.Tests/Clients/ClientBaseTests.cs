using System.Collections.Generic;
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

        protected async Task<ClientContext> LogonWithInvalidCredentialsAsync(string username, string password)
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp = this.AddAuthTokenExpectation(mockHttp);
            mockHttp = this.AddLogonExpectation(mockHttp, username, password, HttpStatusCode.Forbidden);

            var client = this.CreateClient(new RestClient(mockHttp), this.UrlProvider);
            var context = await client.LogonWithCredentialsAsync(username, password);

            mockHttp.VerifyNoOutstandingExpectation();

            return context;
        }

        protected async Task<ClientContext> LogonWithValidCredentialsAsync(string username, string password)
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp = this.AddAuthTokenExpectation(mockHttp);
            mockHttp = this.AddLogonExpectation(mockHttp, username, password, HttpStatusCode.OK);

            var client = this.CreateClient(new RestClient(mockHttp), this.UrlProvider);
            var context = await client.LogonWithCredentialsAsync(username, password);

            mockHttp.VerifyNoOutstandingExpectation();

            return context;
        }

        protected abstract ClientBase<TUrlProvider> CreateClient(RestClient restClient, TUrlProvider urlProvider);

        protected MockHttpMessageHandler AddLogonExpectation(MockHttpMessageHandler mockHttp, string username, string password, HttpStatusCode statusCode)
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
            if (statusCode == HttpStatusCode.OK)
            {
                // when logon is ok, we need to return content
                mockRequest.Respond(statusCode, "application/json", this.JsonSerializer.Serialize(new LogonAuth
                {
                    AuthType = "sa"
                }));

                // when logon is ok, we expect to acquire a session
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
