// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Linq.Expressions;
using EveSwaggerInterfaceClient.ComponentModel;
using Moq;
using Should;
using Xunit;

namespace EveSwaggerInterfaceClient.Tests.UnitTests {
    public class TokenManagerTests {
        private class MockContainer {
            private readonly Expression<Func<TokenType, bool>> _accessToken = t => t == TokenType.AccessToken;
            private readonly Expression<Func<string, bool>> _character = s => s == "CHARACTER";
            private readonly Expression<Func<TokenType, bool>> _refreshToken = t => t == TokenType.RefreshToken;
            public readonly Mock<ITokenPersistence> TokenPersistence = new Mock<ITokenPersistence>();
            public readonly Mock<ITokenService> TokenService = new Mock<ITokenService>();
            public readonly Mock<ITokenValidator> TokenValidator = new Mock<ITokenValidator>();

            public TokenManager CreateService() {
                return new TokenManager(TokenPersistence.Object, TokenService.Object, TokenValidator.Object);
            }

            public MockContainer WithValidAccessToken() {
                SetupToken("ACCESSTOKEN", 1000, true, TokenType.AccessToken);
                return this;
            }

            public MockContainer WithInvalidAccessToken() {
                SetupToken("ACCESSTOKEN", -200, false, TokenType.AccessToken);
                return this;
            }

            public MockContainer WithValidRefreshToken() {
                SetupToken("REFRESHTOKEN", 1000, true, TokenType.RefreshToken);
                return this;
            }

            public MockContainer WithInvalidRefreshToken() {
                SetupToken("REFRESHTOKEN", -200, false, TokenType.RefreshToken);
                return this;
            }

            public MockContainer WithSuccessfulRefresh() {
                TokenService
                    .Setup(x => x.RefreshToken(It.Is<string>(s => s == "REFRESHTOKEN")))
                    .ReturnsAsync(() => new Token {Data = "ACCESSTOKEN", Expiry = DateTime.UtcNow.AddSeconds(1200)})
                    .Verifiable();
                return this;
            }

            public MockContainer WithStoreToken() {
                TokenPersistence
                    .Setup(x => x.Set(It.Is(_accessToken), It.Is(_character), It.IsAny<Token>()))
                    .Verifiable();
                return this;
            }

            private void SetupToken(string data, int expirySeconds, bool isValid, TokenType tokenType) {
                TokenPersistence.Setup(
                        x => x.Get(
                            It.Is<TokenType>(t => t == tokenType),
                            It.Is(_character)))
                    .Returns(new Token {Data = data, Expiry = DateTime.UtcNow.AddSeconds(expirySeconds)})
                    .Verifiable();

                TokenValidator.Setup(
                        x => x.IsValid(It.Is<Token>(t => t.Data == data)))
                    .Returns(isValid)
                    .Verifiable();
            }
        }

        [Fact]
        public void ShouldGetAccessTokenForCharacter() {
            var mocks = new MockContainer()
                .WithValidAccessToken();

            var sut = mocks.CreateService();
            var result = sut.GetToken("CHARACTER").Result;
            result.ShouldEqual("ACCESSTOKEN");
            mocks.TokenPersistence.VerifyAll();
            mocks.TokenValidator.VerifyAll();
        }

        [Fact]
        public void ShouldNotThrow() {
            var dbName = Guid.NewGuid().ToString();
            var sut = new TokenManager(dbName);
            sut.ShouldNotBeNull();
        }

        [Fact]
        public void ShouldRefreshToken() {
            var mocks = new MockContainer()
                .WithInvalidAccessToken()
                .WithValidRefreshToken()
                .WithSuccessfulRefresh()
                .WithStoreToken();

            var sut = mocks.CreateService();
            var result = sut.GetToken("CHARACTER").Result;
            result.ShouldEqual("ACCESSTOKEN");
            mocks.TokenPersistence.VerifyAll();
            mocks.TokenValidator.VerifyAll();
            mocks.TokenService.VerifyAll();
        }

        [Fact]
        public void ShouldThrowIfNoValidAuthenticationMaterial() {
            var mocks = new MockContainer()
                .WithInvalidAccessToken()
                .WithInvalidRefreshToken();

            var sut = mocks.CreateService();
            Action action = () => sut.GetToken("CHARACTER").Wait();
            action.ShouldThrow<AggregateException>();
            mocks.TokenPersistence.VerifyAll();
            mocks.TokenValidator.VerifyAll();
        }
    }
}