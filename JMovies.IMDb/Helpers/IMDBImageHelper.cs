using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JMovies.IMDb.Helpers
{
    public class IMDBImageHelper
    {
        public static string NormalizeImageUrl(string url)
        {
            return Regex.Replace(url, @"._V1.*?.jpg", "._V1._SY0.jpg");
        }
    }
}
