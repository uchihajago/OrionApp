using System.Text;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Core
{
    public static class PhonewordTranslator
    {
        public static string ToNumber(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "";
            else
                raw = raw.ToUpperInvariant();

            var newNumber = new StringBuilder();
            foreach (var c in raw)
            {
                if (" -0123456789".Contains(c))
                {
                    newNumber.Append(c);
                }
                else
                {
                    var result = TranslateToNumber(c);
                    if (result != null)
                        newNumber.Append(result);
                }
                // otherwise we've skipped a non-numeric char
            }
            return newNumber.ToString();
        }
        static bool Contains(this string keyString, char c)
        {
            return keyString.IndexOf(c) >= 0;
        }
        static int? TranslateToNumber(char c)
        {
            if ("ABC".Contains(c))
                return 2;
            else if ("DEF".Contains(c))
                return 3;
            else if ("GHI".Contains(c))
                return 4;
            else if ("JKL".Contains(c))
                return 5;
            else if ("MNO".Contains(c))
                return 6;
            else if ("PQRS".Contains(c))
                return 7;
            else if ("TUV".Contains(c))
                return 8;
            else if ("WXYZ".Contains(c))
                return 9;
            return null;
        }
    }

    public static class RequestHelper
    {
        private static HttpClient client;
        public async static Task<T> PostRequest<T>(this string url, string body)
        {
            if (client == null) { client = new HttpClient(); }
            try
            {
                var content = new StringContent(body, UnicodeEncoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var json = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception) { return default(T); }
        }
        public async static Task<T> GetRequest<T>(string uri)
        {
            if (client == null) { client = new HttpClient(); }
            try
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead);
                var json = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex) {
                ex.Message.ToString();
                return default(T);
            }
        }
        /*
        List<Job> model = null;
        var client = new HttpClient();
        var task = client.GetAsync("http://api.usa.gov/jobs/search.json?query=nursing+jobs")
            .ContinueWith((taskwithresponse) =>
            {
                var response = taskwithresponse.Result;
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                model = JsonConvert.DeserializeObject<List<Job>>(jsonString.Result);

            });
        task.Wait(); 
        
        public async static Task<List<string>> Test()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"http://thebaseadres.net/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("lastbitofurl");
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    List<String> test = JsonConvert.DeserializeObject<List<string>>(s);
                    return test;
                }
                else
                    return null;
            }
        }

         */
    }

    //public class Owner
    //{
    //    public int reputation { get; set; }
    //    public int user_id { get; set; }
    //    public string user_type { get; set; }
    //    public int accept_rate { get; set; }
    //    public string profile_image { get; set; }
    //    public string display_name { get; set; }
    //    public string link { get; set; }
    //}

    //public class Item
    //{
    //    public Owner owner { get; set; }
    //    public bool is_accepted { get; set; }
    //    public int score { get; set; }
    //    public int last_activity_date { get; set; }
    //    public int creation_date { get; set; }
    //    public int answer_id { get; set; }
    //    public int question_id { get; set; }
    //    public int? last_edit_date { get; set; }
    //}

    //public class RootObject
    //{
    //    public List<Item> items { get; set; }
    //    public bool has_more { get; set; }
    //    public int quota_max { get; set; }
    //    public int quota_remaining { get; set; }
    //}

    public class RootObject
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }
}