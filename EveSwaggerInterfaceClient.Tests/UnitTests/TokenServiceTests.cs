// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using EveSwaggerInterfaceClient.ComponentModel;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Should;
using Xunit;

namespace EveSwaggerInterfaceClient.Tests.UnitTests {
    public class TokenServiceTests {
        private static Mock<IConfigurationService> SetupConfigurationService() {
            // Setup Configuration Service
            var configMock = new Mock<IConfigurationService>();
            configMock.Setup(x => x.Get<string>("LoginServerBaseUrl"))
                .Returns("https://login.example.com/oauth")
                .Verifiable();
            configMock.Setup(x => x.Get<string>("ClientID")).Returns("ClientID").Verifiable();
            configMock.Setup(x => x.Get<string>("ClientSecret")).Returns("ClientSecret").Verifiable();
            return configMock;
        }

        [Fact]
        public void DefaultConstructorShouldNotThrow() {
            var sut = new TokenService();
        }

        [Fact]
        public void ShouldMakeHttpRequestToLoginServer() {
            var configMock = SetupConfigurationService();

            // Setup Message Handler
            var body = new Dictionary<string, string> {
                {"access_token", "ACCESSTOKEN"},
                {"expires_in", "1200"}
            };
            var content = JsonConvert.SerializeObject(body);
            var messageHandler = new MockHttpMessageHandler();
            messageHandler
                .Expect(HttpMethod.Post, "https://login.example.com/oauth/token")
                .WithHeaders("Authorization", "Basic Q2xpZW50SUQ6Q2xpZW50U2VjcmV0")
                .WithPartialContent("REFRESHTOKEN")
                .Respond(requestMessage => new StringContent(content));

            // Setup Token Service
            var sut = new TokenService(configMock.Object, messageHandler);
            var accessToken = sut.RefreshToken("REFRESHTOKEN").Result;
            accessToken.Data.ShouldEqual("ACCESSTOKEN");
            accessToken.Expiry.ShouldBeGreaterThan(DateTime.UtcNow.AddSeconds(1150));
            accessToken.Expiry.ShouldBeLessThan(DateTime.UtcNow.AddSeconds(1250));
        }

        [Fact]
        public void ShouldThrowIfNonOkResponse() {
            var configMock = SetupConfigurationService();

            // Setup Message Handler
            var messageHandler = new MockHttpMessageHandler();
            messageHandler
                .Expect(HttpMethod.Post, "https://login.example.com/oauth/token")
                .Respond(HttpStatusCode.NotFound);

            // Setup Token Service
            var sut = new TokenService(configMock.Object, messageHandler);
            Action action = () => sut.RefreshToken("REFRESHTOKEN").Wait();
            action.ShouldThrow<AggregateException>();
        }
    }
}