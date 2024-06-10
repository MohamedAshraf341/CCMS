using System.Net.Http;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiHttpClient
    {
        private readonly HttpClient client;
        public ApiHttpClient(HttpClient _client)
        {
            client = _client;
        }
        public async Task<T> Get<T>(string url)
        {
            try
            {
                using var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        private static JsonContent GetJsContent<T>(T item)
        {
            var jsOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false
            };
            var jsonContent = JsonContent.Create(item, options: jsOptions);
            return jsonContent;
        }
        public async Task PutSimple<T>(string url, T item)
        {
            var jsonContent = GetJsContent(item);

            using var response = await client.PutAsync(url, jsonContent);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
        }
        public async Task<TRes> Put<TReq, TRes>(string url, TReq item)
        {
            var jsonContent = GetJsContent(item);

            using var response = await client.PutAsync(url, jsonContent);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(content);

            var contentString = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TRes>(contentString);
            return result;
        }
        public async Task PostSimple<T>(string url, T item)
        {
            var jsonContent = GetJsContent(item);

            using var response = await client.PostAsync(url, jsonContent);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
        }
        public async Task<TRes> Post<TReq, TRes>(string url, TReq item)
        {
            var jsonContent = GetJsContent(item);

            using var response = await client.PostAsync(url, jsonContent);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(content);

            var contentString = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TRes>(contentString);
            return result;
        }
        internal async Task<TRes> Delete<TRes>(string url)
        {
            using var response = await client.DeleteAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(content);

            var contentString = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TRes>(contentString);
            return result;
        }



    }
}
