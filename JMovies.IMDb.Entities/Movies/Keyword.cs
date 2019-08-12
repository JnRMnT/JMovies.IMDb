using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a keyword related to a title
    /// </summary>
    public class Keyword
    {
        /// <summary>
        /// Keyword name
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Unique Identifier of a keyword
        /// </summary>
        public string Identifier { get; set; }
    }
}
