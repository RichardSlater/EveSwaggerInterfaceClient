// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EveSwaggerInterfaceClient.ComponentModel;
using Newtonsoft.Json.Linq;

namespace EveSwaggerInterfaceClient {
    public class TokenService : ITokenService {
        private readonly IConfigurationService _configService;
        private readonly HttpClient _httpClient;

        public TokenService() {
            _configService = new AppConfigConfiguration();
            _httpClient = new HttpClient();
        }

        public TokenService(IConfigurationService configService, HttpMessageHandler messageHandler) {
            _configService = configService;
            _httpClient = new HttpClient(messageHandler);
        }

        public async Task<Token> RefreshToken(string refreshToken) {
            var body = new Dictionary<string, string> {
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken}
            };
            var clientId = _configService.Get<string>("ClientID");
            var clientSecret = _configService.Get<string>("ClientSecret");
            var credentials = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
            var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            var loginUrl = _configService.Get<string>("LoginServerBaseUrl") + "/token";
            var request = new HttpRequestMessage(HttpMethod.Post, loginUrl);
            request.Headers.Authorization = authHeader;
            request.Content = new FormUrlEncodedContent(body);
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(
                    $"Token Exchange failed, got a {response.StatusCode} back from the login server when a 200 OK was expected.");

            var obj = JObject.Parse(await response.Content.ReadAsStringAsync());
            return new Token {
                Data = obj.SelectToken("access_token").Value<string>(),
                Expiry = DateTime.UtcNow.AddSeconds(obj.SelectToken("expires_in").Value<int>())
            };
        }
    }
}