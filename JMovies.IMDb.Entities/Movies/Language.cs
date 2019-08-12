using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a language
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Name of the language
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unique Identifier of the language
        /// </summary>
        public string Identifier { get; set; }
    }
}
