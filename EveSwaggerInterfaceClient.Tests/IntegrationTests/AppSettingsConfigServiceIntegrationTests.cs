using Should;
using Xunit;

namespace EveSwaggerInterfaceClient.Tests.IntegrationTests {
    public class AppSettingsConfigServiceIntegrationTests {
        [Fact]
        public void ShouldReturnKnownAppConfigValue() {
            var sut = new AppConfigConfiguration();
            sut.Get<string>("TestKey").ShouldEqual("TestValue");
        }
    }
}