using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enterprise.Client
{
    public class HttpClientService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly TypedClient client;

        public HttpClientService(IHttpClientFactory httpClient, TypedClient client)
        {
            this._httpClient = httpClient;
            this.client = client;
        }

        private async Task Get(CancellationToken cancellationToken)
        {
            var httpClient = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "url of endpoint");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
                var dataReturned = stream.ReadAndSerrializeFromJson < List<object>();
            }
        }

        private async Task GetWihNamedClient(CancellationToken cancellationToken)
        {
            var httpClient = _httpClient.CreateClient("Client Name");
            var request = new HttpRequestMessage(HttpMethod.Get, "endpoint name");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
                var dataReturned = stream.ReadAndSerrializeFromJson < List<object>();
            }
        }

        private async Task GetWihTypedClient(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "endpoint name");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await this.client.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
                var dataReturned = stream.ReadAndSerrializeFromJson < List<object>();
            }
        }

        private async Task GetWihTypedScopedClient(CancellationToken cancellationToken)
        {
            await this.client.Get(cancellationToken);
        }

        private async Task GetWihNamedClientWithInvalidReponse(CancellationToken cancellationToken)
        {
            var httpClient = _httpClient.CreateClient("Client Name");
            var request = new HttpRequestMessage(HttpMethod.Get, "endpoint name");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound) { Console.WriteLine("Not Found request"); return; }
                    else if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    {
                        var errorStream = await response.Content.ReadAsStreamAsync();
                        var validationErrors = errorStream.ReadAndDeserailizeFromJson();
                        Console.WriteLine(validationErrors); return;
                    }
                    response.EnsureSuccessStatusCode();
                }
                var stream = await response.Content.ReadAsStreamAsync();

                var dataReturned = stream.ReadAndSerrializeFromJson < List<object>();
            }
        }
    }
}
