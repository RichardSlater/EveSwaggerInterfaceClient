// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

namespace EveSwaggerInterfaceClient {
    public class TokenType {
        public static readonly TokenType RefreshToken = new TokenType(nameof(RefreshToken));
        public static readonly TokenType AccessToken = new TokenType(nameof(AccessToken));

        private TokenType(string tokenKey) {
            Key = tokenKey;
        }

        public string Key { get; }

        public override string ToString() {
            return Key;
        }
    }
}