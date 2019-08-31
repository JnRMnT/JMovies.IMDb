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
        /// <summary>
        /// Constructor by type of the datasource
        /// </summary>
        /// <param name="dataSourceType">Type of the data source</param>
        /// <param name="production">Related Production instance</param>
        public Rating(DataSourceTypeEnum dataSourceType, Production production)
        {
            this.DataSource = new DataSource(dataSourceType);
            this.Production = production;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Rating() : this(DataSourceTypeEnum.Undefined, null)
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
        [ForeignKey("DataSourceID")]
        public virtual DataSource DataSource { get; set; }

        /// <summary>
        /// Reference field for DataSource
        /// </summary>
        public long DataSourceID { get; set; }

        /// <summary>
        /// Related production
        /// </summary>
        [Required]
        [ForeignKey("ProductionID")]
        public virtual Production Production { get; set; }

        /// <summary>
        /// Reference field for Production
        /// </summary>
        public long ProductionID { get; set; }
    }
}
