// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using EveSwaggerInterfaceClient.ComponentModel;

namespace EveSwaggerInterfaceClient {
    public class TokenValidator : ITokenValidator {
        public bool IsValid(Token token) {
            if (string.IsNullOrWhiteSpace(token.Data)) return false;

            if (token.Expiry.HasValue && token.Expiry.Value < DateTime.UtcNow) return false;

            return true;
        }
    }
}