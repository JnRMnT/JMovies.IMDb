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
        public long IMDbID { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        public Rating Rating { get; set; }
        public virtual ProductionTypeEnum ProductionType
        {
            get
            {
                return ProductionTypeEnum.Undefined;
            }
        }
    }
}
