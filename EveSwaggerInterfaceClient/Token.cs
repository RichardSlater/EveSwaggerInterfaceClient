// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;

namespace EveSwaggerInterfaceClient {
    [Serializable]
    public struct Token {
        public DateTime? Expiry { get; set; }
        public string Data { get; set; }

        public static Token FromRefreshToken(string data) {
            return new Token {Data = data, Expiry = DateTime.MaxValue};
        }

        public static Token FromAccessToken(string data, DateTime expiry) {
            return new Token {Data = data, Expiry = expiry};
        }
    }
}