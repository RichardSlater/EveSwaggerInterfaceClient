// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Collections.Generic;
using EveSwaggerInterfaceClient.ComponentModel;
using Microsoft.Isam.Esent.Collections.Generic;

namespace EveSwaggerInterfaceClient {
    public class EsentTokenPersistence : ITokenPersistence {
        private static readonly Dictionary<string, EsentTokenPersistence> Instances =
            new Dictionary<string, EsentTokenPersistence>();

        private readonly PersistentDictionary<string, Token> _dictionary;

        public EsentTokenPersistence(string databaseName) {
            if (Instances.ContainsKey(databaseName))
                throw new InvalidOperationException(
                    $"An instance of {nameof(EsentTokenPersistence)} already exists with the name {databaseName}, only one instance of each token persistence layer may exist at one time.");
            _dictionary = new PersistentDictionary<string, Token>(databaseName);
            Instances.Add(databaseName, this);
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

        public static EsentTokenPersistence Get(string databaseName) {
            if (Instances.ContainsKey(databaseName)) return Instances[databaseName];

            return new EsentTokenPersistence(databaseName);
        }

        private static string GenerateKey(TokenType tokenType, string character) {
            return $"{tokenType.Key}_{character}";
        }
    }
}