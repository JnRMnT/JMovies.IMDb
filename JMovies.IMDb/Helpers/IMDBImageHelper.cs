using System.Net;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers
{
    /// <summary>
    /// Class responsible for providing utility methods related to Image related operations for IMDb
    /// </summary>
    public class IMDBImageHelper
    {
        /// <summary>
        /// Class responsible for normalizing image urls within IMDb. By default IMDb uses a handler to return cropped versions of the images. This method ensures it returns the raw image.
        /// </summary>
        /// <param name="url">URL of the image</param>
        /// <returns>Normalized URL of the image</returns>
        public static string NormalizeImageUrl(string url)
        {
            string normalizedURL = Regex.Replace(url, @"._V1.*?.jpg", "._V1._SY0.jpg");
            if (normalizedURL.EndsWith("@"))
            {
                normalizedURL += "._V1._SY0.jpg";
            }
            return normalizedURL;
        }

        /// <summary>
        /// Fetches the content of an image using its URL
        /// </summary>
        /// <param name="url">URL of the image to be fetched</param>
        /// <returns>Byte array content of the image</returns>
        public static byte[] GetImageContent(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadData(url);
            }
        }
    }
}
