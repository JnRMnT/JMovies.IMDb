using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a company related to the industry
    /// </summary>
    public class Company
    {
        /// <summary>
        /// ID of the company
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Name of the company
        /// </summary>
        public string Name { get; set; }
    }
}
