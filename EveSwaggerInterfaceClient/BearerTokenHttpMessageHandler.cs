using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EveSwaggerInterfaceClient
{
    public class BearerTokenHttpMessageHandler : DelegatingHandler
    {
        private readonly Func<string> _tokenFactory;

        public BearerTokenHttpMessageHandler(Func<string> tokenFactory) : base(new HttpClientHandler())
        {
            _tokenFactory = tokenFactory;
        }

        public BearerTokenHttpMessageHandler(HttpMessageHandler messageHandler, Func<string> tokenFactory) : base(messageHandler)
        {
            _tokenFactory = tokenFactory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenFactory());
            return base.SendAsync(request, cancellationToken);
        }
    }
}
