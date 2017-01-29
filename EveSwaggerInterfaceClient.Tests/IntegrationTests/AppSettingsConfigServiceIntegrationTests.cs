// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

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