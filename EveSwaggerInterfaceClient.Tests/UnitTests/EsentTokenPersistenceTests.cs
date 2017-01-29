// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using Microsoft.Isam.Esent.Collections.Generic;
using Xunit;

namespace EveSwaggerInterfaceClient.Tests.UnitTests {
    public class EsentTokenPersistenceTests {
        [Fact]
        public void ShouldCreateNewDatabase() {
            var databaseName = Guid.NewGuid().ToString();
            Assert.False(PersistentDictionaryFile.Exists(databaseName));
            var sut = new EsentTokenPersistence(databaseName);
            var testToken = Token.FromRefreshToken("TOKENDATA");
            sut.Set(TokenType.RefreshToken, "DUMMYCHAR", testToken);
            Assert.True(PersistentDictionaryFile.Exists(databaseName));
        }

        [Fact]
        public void ShouldNotThrowWhenUsingDefaultConstructor() {
            var sut = new EsentTokenPersistence();
        }

        [Fact]
        public void ShouldStoreAndRetreieveValue() {
            var databaseName = Guid.NewGuid().ToString();
            var sut = new EsentTokenPersistence(databaseName);
            var testToken = Token.FromRefreshToken("TOKENDATA");
            sut.Set(TokenType.RefreshToken, "DUMMYCHAR", testToken);
            var output = sut.Get(TokenType.RefreshToken, "DUMMYCHAR");
            Assert.Equal(testToken.Expiry, output.Expiry);
            Assert.Equal(testToken.Data, output.Data);
        }
    }
}