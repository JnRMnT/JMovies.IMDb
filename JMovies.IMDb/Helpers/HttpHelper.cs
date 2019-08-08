using JMovies.IMDb.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Helpers
{
    public class HttpHelper
    {
        public static WebRequest InitializeWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.Headers["Accept-Language"] = IMDbConstants.DefaultScrapingCulture;
            Random random = new Random();
            //Random IP Address
            webRequest.Headers["X-Forwarded-For"] = random.Next(0, 255) + "." + random.Next(0, 255) + "." + random.Next(0, 255) + "." + random.Next(0, 255);
            //Random User-Agent
            webRequest.UserAgent = "Mozilla/" + random.Next(3, 5) + ".0 (Windows NT " + random.Next(3, 5) + "." + random.Next(0, 2) + "; rv:2.0.1) Gecko/20100101 Firefox/" + random.Next(3, 5) + "." + random.Next(0, 5) + "." + random.Next(0, 5);

            return webRequest;
        }
    }
}
