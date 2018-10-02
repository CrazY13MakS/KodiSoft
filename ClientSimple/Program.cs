using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClientSimple
{
    class Program
    {
        static String url = @"https://localhost:44317/";
        static String email = "test111@mail.com";
        static String password = "Qwerty!23";
        static HttpClient httpClient = new HttpClient();
        static String feed_1 = "https://www.liga.net/biz/all/rss.xml";
        static String feed_2 = "http://feeds.bbci.co.uk/news/world/rss.xml"; 
        static String feed_3 = "https://www.theregister.co.uk/business/channel/headlines.atom";
        static String token;
        static int collectionId;
        static void Main(string[] args)
        {
            String feed = "";
            bool res = Login().Result;
            if (!res)
            {
                res = Reg().Result;
            }
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (res)
            {
                res = CreateCollection().Result;

            }
            if (res)
            {
                res = AddFeedToCollection(feed_1).Result;
            }
            if (res)
            {
                res = AddFeedToCollection(feed_2).Result;
            }
            if (res)
            {
                res = AddFeedToCollection(feed_3).Result;
            }
            if (res)
            {
                feed = ReadNews(collectionId).Result;
                Console.WriteLine(feed);
            }
            Console.ReadKey();
        }
        private static async Task<String> ReadNews(int collectionId)
        {
            var response = await httpClient.GetAsync(url + "news/" + collectionId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return "";
        }
        private static async Task<bool> AddFeedToCollection(String uri)
        {
            var response = await httpClient.PostAsync(url + "addfeed", new JsonContent(new { Id = collectionId, Uri = uri }));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        private static async Task<bool> CreateCollection()
        {
            var response = await httpClient.PostAsync(url + "createcollection", new JsonContent("firstFeeeeeeed"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (int.TryParse(await response.Content.ReadAsStringAsync(), out collectionId))
                {
                    return true;
                }
            }
            return false;
        }
        private static async Task<bool> Reg()
        {
            var response = await httpClient.PostAsync(url + "registration", new JsonContent(new { email = email, password = password, confirmPassword = password }));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var resource = File.CreateText("token.txt"))
                {
                    token = await response.Content.ReadAsStringAsync();
                    resource.Write(token);
                }
                return true;
            }
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return false;
        }
        private static async Task<bool> Login()
        {
            var response = await httpClient.PostAsync(url + "login", new JsonContent(new { email = email, password = password, confirmPassword = password }));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var resource = File.CreateText("token.txt"))
                {
                    token = await response.Content.ReadAsStringAsync();
                    resource.Write(token);
                }
                return true;
            }
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return false;
        }

    }
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        { }
    }



}
