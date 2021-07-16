using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace PoeAcolyte.Service
{
    public class PoeHttpService
    {
        public string GetStashRaw()
        {
            try
            {
                var cookies = new CookieContainer();
                cookies.Add(new Cookie("POESESSID", "b20bb076bb3305660acf6a79aa52d1e3" + "d", "/", ".pathofexile.com"));
                string url =
                    @"https://www.pathofexile.com/character-window/get-stash-items?league=Ultimatum&tabs=0&tabIndex=1&accountName=eternal81_2";

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new System.Uri(url));
                request.CookieContainer = cookies;
                var response = request.GetResponse();
                string result = "";
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }

                return result;
            }
            catch (Exception)
            {
                Debug.Print("GetRawStash Failed");
                return "";
            }
        }
    }
}