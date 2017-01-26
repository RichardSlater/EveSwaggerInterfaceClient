using System;
using System.Net.Http;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace EveSwaggerInterfaceClient.Tests.UnitTests
{
    public class BearerTokenHttpMessageHandlerTests
    {
        [Fact]
        public void AttachesBearerTokenToRequest()
        {
            const string uri = "http://example.com/";
            var innerHandler = new MockHttpMessageHandler();

            innerHandler
                .Expect(HttpMethod.Get, uri)
                .WithHeaders("Authorization", "Bearer DUMMYTOKEN");

            var handler = new BearerTokenHttpMessageHandler(innerHandler, () => "DUMMYTOKEN");
            var invoker = new HttpMessageInvoker(handler);
            var message = new HttpRequestMessage(HttpMethod.Get, new Uri(uri));
            invoker.SendAsync(message, new System.Threading.CancellationToken());

            innerHandler.VerifyNoOutstandingExpectation();
        }
    }
}