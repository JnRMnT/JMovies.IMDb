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
    /// Class definition of a country
    /// </summary>
    [Table("Country")]
    public class Country
    {

        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public long ID { get; set; }
        /// <summary>
        /// Name of the country
        /// </summary>
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier of the country
        /// </summary>
        [MaxLength(64)]
        public string Identifier { get; set; }
    }
}
