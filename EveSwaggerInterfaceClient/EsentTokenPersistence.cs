// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using EveSwaggerInterfaceClient.ComponentModel;
using Microsoft.Isam.Esent.Collections.Generic;

namespace EveSwaggerInterfaceClient {
    public class EsentTokenPersistence : ITokenPersistence {
        private readonly PersistentDictionary<string, Token> _dictionary;

        public EsentTokenPersistence(string databaseName) {
            _dictionary = new PersistentDictionary<string, Token>(databaseName);
        }

        public EsentTokenPersistence() : this("Tokens") {}

        public Token Get(TokenType tokenType, string character) {
            Token token;
            _dictionary.TryGetValue(GenerateKey(tokenType, character), out token);
            return token;
        }

        public void Set(TokenType tokenType, string character, Token token) {
            _dictionary[GenerateKey(tokenType, character)] = token;
        }

        private static string GenerateKey(TokenType tokenType, string character) {
            return $"{tokenType.Key}_{character}";
        }
    }
}