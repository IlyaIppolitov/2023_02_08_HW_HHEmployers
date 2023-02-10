using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace HH_API
{

    public static class HeadhunterClient
    {

        public static async Task<EmployersResponse> GetEmployersAsync(string companyName)
        {
            if (string.IsNullOrEmpty(companyName)) throw new ArgumentNullException($"Пуская строка поиска!");

            string uri = @"https://api.hh.ru/employers?text=" + companyName;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "AppNumber1");

            using var response = await httpClient.GetAsync(uri).ConfigureAwait(false);

            EmployersResponse employers = new EmployersResponse();
            
            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errors = await response.Content.ReadFromJsonAsync<HH_EmployersBadRespond>();
                throw new HttpResponseException(errors.ToString);

            }
            else if (response.IsSuccessStatusCode)
            {
                employers = await response.Content.ReadFromJsonAsync<EmployersResponse>();
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
            return employers;
        }

        public static async Task<EmplyerInfo> GetEmployerInfoAsync(string companyUrl)
        {
            if (!Uri.IsWellFormedUriString(companyUrl, UriKind.Absolute)) throw new HttpRequestException($"Неверная строка запроса!");

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "AppNumber1");

            using var response = await httpClient.GetAsync(companyUrl).ConfigureAwait(false);

            EmplyerInfo employerInfo = new EmplyerInfo();

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errors = await response.Content.ReadFromJsonAsync<HH_ErrorNoSuchEmployer>();
                throw new HttpResponseException(errors.ToString);

            }
            else if (response.IsSuccessStatusCode)
            {
                employerInfo = await response.Content.ReadFromJsonAsync<EmplyerInfo>();
            }
            else
            {
                response.EnsureSuccessStatusCode();
            }
            return employerInfo;
        }


        [Serializable]
        private class HttpResponseException : Exception
        {
            private Func<string> toString;

            public HttpResponseException()
            {
            }

            public HttpResponseException(Func<string> toString)
            {
                this.toString = toString;
            }

            public HttpResponseException(string? message) : base(message)
            {
            }

            public HttpResponseException(string? message, Exception? innerException) : base(message, innerException)
            {
            }

            protected HttpResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}