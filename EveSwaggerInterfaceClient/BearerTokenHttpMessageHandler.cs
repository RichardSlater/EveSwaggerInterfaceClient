// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EveSwaggerInterfaceClient {
    public class BearerTokenHttpMessageHandler : DelegatingHandler {
        private readonly Func<string> _tokenFactory;

        public BearerTokenHttpMessageHandler(Func<string> tokenFactory) : base(new HttpClientHandler()) {
            _tokenFactory = tokenFactory;
        }

        public BearerTokenHttpMessageHandler(HttpMessageHandler messageHandler, Func<string> tokenFactory)
            : base(messageHandler) {
            _tokenFactory = tokenFactory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken) {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenFactory());
            return base.SendAsync(request, cancellationToken);
        }
    }
}