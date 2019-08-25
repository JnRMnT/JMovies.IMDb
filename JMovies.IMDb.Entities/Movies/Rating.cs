using JMovies.IMDb.Entities.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of Ratings
    /// </summary>
    [Table("Rating")]
    public class Rating
    {
        public Rating(DataSourceTypeEnum dataSourceType)
        {
            this.DataSource = new DataSource(dataSourceType);
        }

        public Rating() : this(DataSourceTypeEnum.Undefined)
        {

        }

        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// Value of the rating
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Count of rates
        /// </summary>
        public long RateCount { get; set; }

        /// <summary>
        /// Source of the rating data
        /// </summary>
        public virtual DataSource DataSource { get; set; }
    }
}
