using JMovies.IMDb.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMovies.IMDb.Entities.People
{
    /// <summary>
    /// Credit of a person in a specific production
    /// </summary>
    public class ProductionCredit
    {
        /// <summary>
        /// Credit
        /// </summary>
        public Credit Credit { get; set; }

        /// <summary>
        /// The production that person is credited in
        /// </summary>
        public Production Production { get; set; }
    }
}
