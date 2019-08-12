using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a genre
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// Name of the Genre
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Unique identifier of the Genre
        /// </summary>
        public string Identifier { get; set; }
    }
}
