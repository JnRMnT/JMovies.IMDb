using System;
using System.Collections.Generic;
using System.Text;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of Ratings
    /// </summary>
    public class Rating
    {
        /// <summary>
        /// Value of the rating
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Count of rates
        /// </summary>
        public long RateCount { get; set; }
    }
}
