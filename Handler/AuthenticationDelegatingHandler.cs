using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FoodMvc.Handler
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ClientCredentialsTokenRequest _tokenRequest;

        public AuthenticationDelegatingHandler(IHttpClientFactory _httpClientFactory, ClientCredentialsTokenRequest _tokenRequest)
        {
            this._httpClientFactory = _httpClientFactory;
            this._tokenRequest = _tokenRequest;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestt, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("IDPClient");

            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(_tokenRequest);
            if (tokenResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the access token");
            }

            requestt.SetBearerToken(tokenResponse.AccessToken);

            return await base.SendAsync(requestt, cancellationToken);

        }
    }
}
