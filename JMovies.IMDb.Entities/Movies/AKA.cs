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
    /// Class definition for other names of the title
    /// </summary>
    [Table("AKA")]
    public class AKA
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public long ID { get; set; }
        /// <summary>
        /// Description of the alternative name
        /// </summary>
        [MaxLength(64)]
        [Column(Order = 1)]
        public string Description { get; set; }

        /// <summary>
        /// Alternative name of the Title
        /// </summary>
        [MaxLength(128)]
        [Column(Order = 2)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Reference field for Production
        /// </summary>
        public long ProductionID { get; set; }
    }
}
