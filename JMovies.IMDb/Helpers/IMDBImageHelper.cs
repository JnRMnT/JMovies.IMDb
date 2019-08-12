using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            return Regex.Replace(url, @"._V1.*?.jpg", "._V1._SY0.jpg");
        }
    }
}
