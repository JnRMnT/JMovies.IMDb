using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of release dates
    /// </summary>
    public class ReleaseDate
    {
        /// <summary>
        /// Date of the release
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Country of release date
        /// </summary>
        public Country Country { get; set; }
    }
}
