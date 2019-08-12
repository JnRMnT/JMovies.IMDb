using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a character in a title
    /// </summary>
    public class Character
    {
        /// <summary>
        /// IMDb ID
        /// </summary>
        public long? IMDbID { get; set; }

        /// <summary>
        /// Name of the character
        /// </summary>
        public string Name { get; set; }
    }
}
