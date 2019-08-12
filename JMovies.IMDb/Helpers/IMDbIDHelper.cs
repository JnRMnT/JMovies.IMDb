using JMovies.IMDb.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers
{
    /// <summary>
    /// Class responsible for providing utility methods related to IMDB ID related operations
    /// </summary>
    public class IMDBIDHelper
    {
        /// <summary>
        /// Method responsible for getting IMDb ID from a URL
        /// </summary>
        /// <param name="url">URL to be parsed</param>
        /// <returns>Detected IMDb ID</returns>
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
