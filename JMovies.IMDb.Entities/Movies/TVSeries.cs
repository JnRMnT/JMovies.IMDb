using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of TV Series
    /// </summary>
    [Table("TVSeries")]
    public class TVSeries: Movie
    {
        /// <summary>
        /// End Year of the series
        /// </summary>
        [MaxLength(4)]
        public int? EndYear { get; set; }

        /// <summary>
        /// Type of the production
        /// </summary>
        public override ProductionTypeEnum ProductionType => ProductionTypeEnum.TVSeries;
    }
}
