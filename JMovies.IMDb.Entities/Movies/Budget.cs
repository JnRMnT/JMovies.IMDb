using JMovies.IMDb.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of Budget of a Title
    /// </summary>
    public class Budget
    {
        /// <summary>
        /// Description related to the budget information
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Amount of the budget of the title
        /// </summary>
        public Amount Amount { get; set; }
        
    }
}
