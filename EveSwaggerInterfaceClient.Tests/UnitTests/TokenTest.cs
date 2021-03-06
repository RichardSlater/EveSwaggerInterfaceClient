﻿// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using Should;
using Xunit;

namespace EveSwaggerInterfaceClient.Tests.UnitTests {
    public class TokenTests {
        private readonly DateTime _pointInTime = new DateTime(2012, 06, 13, 16, 42, 21);

        [Fact]
        public void ShouldCreateTokenFromAccessToken() {
            var sut = Token.FromAccessToken("DATA", _pointInTime);
            sut.Data.ShouldEqual("DATA");
            sut.Expiry.ShouldEqual(_pointInTime);
        }

        [Fact]
        public void ShouldCreateTokenFromRefreshToken() {
            var sut = Token.FromRefreshToken("DATA");
            sut.Data.ShouldEqual("DATA");
            sut.Expiry.ShouldEqual(DateTime.MaxValue);
        }
    }
}