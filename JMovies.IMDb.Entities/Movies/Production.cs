using System;
using System.Collections.Generic;
using System.Text;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Base class of any production
    /// </summary>
    public class Production
    {
        /// <summary>
        /// IMDb ID of the production
        /// </summary>
        public long IMDbID { get; set; }

        /// <summary>
        /// Title of the production
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Production Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Current Rating of the production
        /// </summary>
        public Rating Rating { get; set; }

        /// <summary>
        /// Type of the Production
        /// </summary>
        public virtual ProductionTypeEnum ProductionType
        {
            get
            {
                return ProductionTypeEnum.Undefined;
            }
        }
    }
}
