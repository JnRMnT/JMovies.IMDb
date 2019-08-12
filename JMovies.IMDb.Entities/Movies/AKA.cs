using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition for other names of the title
    /// </summary>
    public class AKA
    {
        /// <summary>
        /// Description of the alternative name
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Alternative name of the Title
        /// </summary>
        public string Name { get; set; }
    }
}
