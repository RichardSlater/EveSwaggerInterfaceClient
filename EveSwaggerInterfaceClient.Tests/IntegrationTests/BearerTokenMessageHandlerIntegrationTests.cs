using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EveSwaggerInterfaceClient.Tests.IntegrationTests {
    public class BearerTokenMessageHandlerIntegrationTests {
        [Fact]
        public void ShouldGetSkills()
        {
            // this is a dumb token factory that simply acquires an Access Token for an Alt from a Refresh Token.
            Func<string> tokenFactory = () => TokenOperations.FromRefreshToken(Properties.Settings.Default.RefreshToken);

            var client = new HttpClient(new BearerTokenHttpMessageHandler(tokenFactory));
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri("https://esi.tech.ccp.is/latest/characters/803544995/skills/"));
            var result = client.SendAsync(message).Result;
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var content = result.Content.ReadAsStringAsync().Result;
            Assert.Contains("\"skill_id\": 3380", content);
        }
    }
}
