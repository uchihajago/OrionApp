using HorizonCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HorizonCode
{
    public class RequestHelper
    {
        private readonly static HttpClient _http = new HttpClient();
        public ISerializerJSON SerializerJSON { get; }

        static RequestHelper() { _http.DefaultRequestHeaders.ExpectContinue = false; }
        public RequestHelper() {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private IDictionary<string, string> GetHeaders(Guid requestId)
        {
            return new Dictionary<string, string>
            {
                { "RequestId", requestId.ToString() }
            };
        }

        private static HttpRequestMessage GetExecute(string fullUrl, IEnumerable<KeyValuePair<string, string>> headers, HttpMethod method, StringContent content = null)
        {
            var request = new HttpRequestMessage(method, fullUrl) { Content = content };
            if (method != HttpMethod.Post)
            {
                foreach (var item in headers) { request.Headers.Add(item.Key, item.Value); }
            }
            return request;
        }

        private async Task<HttpResponseMessage> CreateRequestAsync(string resource, HttpMethod method, IDictionary<string, string> headers = null)
        {
            return await CreateRequestAsync<object>(resource, null, method, headers);
        }

        private async Task<HttpResponseMessage> CreateRequestAsync<T>(string resource, T value, HttpMethod method, IDictionary<string, string> headers = null)
        {
            StringContent content = null;
            if (value != null)
            {
                string json = SerializerJSON.Serialize<T>(value);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            return await ExecuteAsync(resource,  method, headers, content);
        }

        private async Task<HttpResponseMessage> ExecuteAsync(string fullUrl, HttpMethod method, IDictionary<string, string> headers = null,  StringContent content = null)
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>(2);
            }
            //headers.Add("MerchantId", Merchant.Id.ToString());
            //headers.Add("MerchantKey", Merchant.Key);
            //var tokenSource = new CancellationTokenSource(_timeOut);
            try
            {
                HttpMethod httpMethod = null;
                if (method == HttpMethod.Post)
                {
                    httpMethod = HttpMethod.Post;
                    //if (headers != null)
                    //{
                    //    foreach (var item in headers)
                    //    {
                    //        content.Headers.Add(item.Key, item.Value);
                    //    }
                    //}
                }
                else if (method == HttpMethod.Get)
                {
                    httpMethod = HttpMethod.Get;
                }
                else if (method == HttpMethod.Put)
                {
                    httpMethod = HttpMethod.Put;
                }
                else if (method == HttpMethod.Delete)
                {
                    httpMethod = HttpMethod.Delete;
                }
                var request = GetExecute(fullUrl, headers, httpMethod, content);
                return await _http.SendAsync(request).ConfigureAwait(false);
            }

            catch (OperationCanceledException e)
            {
                return null;
               // throw new CancellationTokenException(e);
            }
            //finally
            //{
            //    //tokenSource.Dispose();
            //}
        }

        //public async static Task<T> PostRequest<T>(this string url, string body)
        //{
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    try
        //    {
        //        var content = new StringContent(body, UnicodeEncoding.UTF8, "application/json");
        //        var response = await client.PostAsync(url, content);
        //        var json = await response.Content.ReadAsStringAsync();
        //        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        //    }
        //    catch (Exception) { return default(T); }
        //}
        //public async static Task<T> GetRequest<T>(string uri)
        //{
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    try
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead);
        //        var json = await response.Content.ReadAsStringAsync();
        //        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        //    }
        //    catch (Exception) { return default(T); }
        //}
    }
}