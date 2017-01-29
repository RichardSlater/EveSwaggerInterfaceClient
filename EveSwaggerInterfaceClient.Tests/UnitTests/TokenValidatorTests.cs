// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using Should;
using Xunit;

namespace EveSwaggerInterfaceClient.Tests.UnitTests {
    public class TokenValidatorTests {
        [Fact]
        public void ShouldNotValidateEmptyToken() {
            var validator = new TokenValidator();
            var token = new Token {Data = string.Empty};
            validator.IsValid(token).ShouldBeFalse();
        }

        [Fact]
        public void ShouldNotValidateExpiredToken() {
            var validator = new TokenValidator();
            var token = new Token {Data = "TOKENMATERIAL", Expiry = DateTime.UtcNow.AddHours(-1)};
            validator.IsValid(token).ShouldBeFalse();
        }

        [Fact]
        public void ShouldNotValidateWhitespaceToken() {
            var validator = new TokenValidator();
            var token = new Token {Data = "  "};
            validator.IsValid(token).ShouldBeFalse();
        }

        [Fact]
        public void ShouldValidateTokenWithinExpiry() {
            var validator = new TokenValidator();
            var token = new Token {Data = "TOKENMATERIAL", Expiry = DateTime.UtcNow.AddMinutes(20)};
            validator.IsValid(token).ShouldBeTrue();
        }

        [Fact]
        public void ShouldValidateTokenWithoutExpiry() {
            var validator = new TokenValidator();
            var token = new Token {Data = "TOKENMATERIAL", Expiry = null};
            validator.IsValid(token).ShouldBeTrue();
        }
    }
}