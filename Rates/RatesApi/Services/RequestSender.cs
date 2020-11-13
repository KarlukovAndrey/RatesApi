using System.Net.Http;

namespace RatesApi
{
    public class RequestSender
    {
        HttpClient _client;
        public RequestSender( )
        {
            _client = new HttpClient();
        }

        public T Get<T>(string url)
        {
            var response = _client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<T>().Result;
        }
    }
}
