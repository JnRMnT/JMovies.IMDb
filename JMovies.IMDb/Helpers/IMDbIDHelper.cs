using JMovies.IMDb.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers
{
    public class IMDBIDHelper
    {
        public static long? GetIDFromUrl(string url)
        {
            Match idMatch = IMDbConstants.IMDBIDRegex.Match(url);
            long imdbId;
            if (idMatch.Success && long.TryParse(idMatch.Groups[2].Value, out imdbId))
            {
                return imdbId;
            }
            else
            {
                return null;
            }
        }
    }
}
