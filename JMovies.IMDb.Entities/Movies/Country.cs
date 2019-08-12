using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a country
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Name of the country
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier of the country
        /// </summary>
        public string Identifier { get; set; }
    }
}
