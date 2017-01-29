// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System.Security.Authentication;
using System.Threading.Tasks;
using EveSwaggerInterfaceClient.ComponentModel;

namespace EveSwaggerInterfaceClient {
    public class TokenManager : ITokenManager {
        private readonly ITokenPersistence _tokenPersistence;
        private readonly ITokenService _tokenService;
        private readonly ITokenValidator _tokenValidator;

        public TokenManager(string databaseName = "Tokens") {
            _tokenService = new TokenService();
            _tokenPersistence = new EsentTokenPersistence(databaseName);
            _tokenValidator = new TokenValidator();
        }

        public TokenManager(ITokenPersistence tokenPersistence, ITokenService tokenService,
                            ITokenValidator tokenValidator) {
            _tokenService = tokenService;
            _tokenValidator = tokenValidator;
            _tokenPersistence = tokenPersistence;
        }

        public async Task<string> GetToken(string character) {
            var accessToken = _tokenPersistence.Get(TokenType.AccessToken, character);
            var refreshToken = _tokenPersistence.Get(TokenType.RefreshToken, character);

            if (IsValid(accessToken)) return accessToken.Data;

            if (IsValid(refreshToken)) {
                accessToken = await _tokenService.RefreshToken(refreshToken.Data);
                _tokenPersistence.Set(TokenType.AccessToken, character, accessToken);
                return accessToken.Data;
            }

            throw new AuthenticationException("Neither the Access Token or the Refresh Token were valid.");
        }

        private bool IsValid(Token token) {
            return _tokenValidator.IsValid(token);
        }
    }
}