using System.Collections.Generic;
using System.Net.Http;
using EveSwaggerInterfaceClient.Tests.Properties;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace EveSwaggerInterfaceClient.Tests {
    public static class TokenOperations {
        public static string FromRefreshToken(string refreshToken) {
            var body = new Dictionary<string, string> {
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken}
            };
            return GetToken(body);
        }

        private static string GetToken(Dictionary<string, string> body) {
            var settings = Settings.Default;
            var content = new FormUrlEncodedContent(body);
            var result = settings.LoginServerBaseUrl
                .AppendPathSegment("token")
                .WithBasicAuth(settings.ClientID, settings.ClientSecret)
                .PostAsync(content)
                .ReceiveString()
                .Result;

            var obj = JObject.Parse(result);
            return obj.SelectToken("access_token").Value<string>();
        }
    }
}