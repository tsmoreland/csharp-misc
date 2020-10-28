//
// Copyright © 2020 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;

namespace IdentityDemo.App.Services
{
    public sealed class TokenManager : ITokenManager
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenProvider _tokenProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// Instantiates a new instance of <see cref="TokenManager"/> class.
        /// </summary>
        /// <param name="clientFactory">HttpClientFactory used to refresh tokens</param>
        /// <param name="tokenProvider">token provider used to get current tokens and expiry time</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="clientFactory"/> or <param name="tokenProvider"> are null</param>
        /// </exception>
        public TokenManager(IHttpClientFactory clientFactory, ITokenProvider tokenProvider, ILogger<TokenManager> logger)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetAcccessTokenOrEmptyAsync()
        {
            if (AccessTokenIsValid())
                return _tokenProvider.AccessToken;

            var idpClient = _clientFactory.CreateClient();
            var discoveryResponse = await idpClient.GetDiscoveryDocumentAsync("https://localhost:44377"); // address should come from configuration
            if (discoveryResponse.IsError)
                return LogAndReturnEmpty(discoveryResponse);
            var refreshResponse = await idpClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest
                {
                    Address = discoveryResponse.TokenEndpoint,
                    ClientId = "identitydemoapp", // move to configuration
                    ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A", // move to configuration
                    RefreshToken = _tokenProvider.RefreshToken,
                });
            if (refreshResponse.IsError)
                return LogAndReturnEmpty(refreshResponse);

            SetOAuthTokens(refreshResponse);

            return _tokenProvider.AccessToken;
        }

        private bool AccessTokenIsValid() =>
            _tokenProvider.ExpiresAt != DateTime.MinValue &&
            _tokenProvider.ExpiresAt.AddSeconds(-60).ToUniversalTime() > DateTimeOffset.UtcNow;

        private void SetOAuthTokens(TokenResponse response)
        {
            _tokenProvider.SetOAuthTokens(response.AccessToken, response.RefreshToken,
                DateTime.UtcNow.AddSeconds(response.ExpiresIn));
        }

        private string LogAndReturnEmpty(ProtocolResponse response) =>
            LogAndReturnEmpty(response.Error, response.Exception);
        private string LogAndReturnEmpty(string error, Exception exception)
        {
            _logger.LogError(exception,
                !string.IsNullOrEmpty(error)
                    ? $"Failed to refresh access token: {error}"
                    : "Failed to refresh access token");
            return string.Empty;
        }

    }
}
