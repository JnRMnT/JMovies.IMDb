using JMovies.IMDb.Common.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JMovies.IMDb.Helpers
{
    public class HttpHelper
    {
        /// <summary>
        /// Initializes the web request using the URL and configures the request for default language & dummy User Agent/IP Info
        /// </summary>
        /// <param name="url">URL of the Request</param>
        /// <returns>Configured Web Request</returns>
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

        /// <summary>
        /// Retry mechanism for sending Web Request that is required to avoid IMDB blocks
        /// </summary>
        /// <param name="webRequest">Request to be sent</param>
        /// <param name="retryCount">Current Retry Count</param>
        /// <returns>Successful Response Stream</returns>
        public static Stream GetResponseStream(WebRequest webRequest, int retryCount = 0)
        {
            //Wait for some time to avoid connectivity issues
            if (retryCount != 0)
            {
                Thread.Sleep(IMDbConstants.HttpRetrySleepTime * retryCount);
            }

            //Apply retry mechanism
            try
            {
                return webRequest.GetResponse().GetResponseStream();
            }
            catch
            {
                if (retryCount + 1 <= IMDbConstants.HttpMaxRetryCount)
                {
                    webRequest = InitializeWebRequest(webRequest.RequestUri.AbsoluteUri);
                    return GetResponseStream(webRequest, retryCount + 1);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
