using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.Movies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.People
{
    /// <summary>
    /// Class definition of a person
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// Type of the underlining person
        /// </summary>
        [Required]
        [MaxLength(4)]
        [NotMapped]
        public virtual PersonTypeEnum PersonType { get; set; }
        /// <summary>
        /// IMDb ID of the person
        /// </summary>
        public long IMDbID { get; set; }

        /// <summary>
        /// Full Name of the person
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string FullName { get; set; }

        /// <summary>
        /// Main image of the person
        /// </summary>
        [MaxLength(255)]
        public virtual Image PrimaryImage { get; set; }

        /// <summary>
        /// Roles of the person
        /// </summary>
        [MaxLength(128)]
        public ICollection<CreditRoleType> Roles { get; set; }

        /// <summary>
        /// Birth Date of the person
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Birth Place of the person
        /// </summary>
        [MaxLength(128)]
        public string BirthPlace { get; set; }

        /// <summary>
        /// Birth Place of the person
        /// </summary>
        [MaxLength(128)]
        public string BirthName { get; set; }

        /// <summary>
        /// Height of the person
        /// </summary>
        [MaxLength(4)]
        public Length Height { get; set; }

        /// <summary>
        /// NickName of the person
        /// </summary>
        [MaxLength(128)]
        public string NickName { get; set; }

        /// <summary>
        /// Mini Biography of the person
        /// </summary>
        public string MiniBiography { get; set; }

        /// <summary>
        /// Available Photos of the person
        /// </summary>
        public virtual ICollection<Image> Photos { get; set; }

        /// <summary>
        /// Known credits of the person
        /// </summary>
        [ForeignKey("PersonID")]
        public virtual ICollection<ProductionCredit> KnownFor { get; set; }

        /// <summary>
        /// Gender of the person
        /// </summary>
        [MaxLength(2)]
        public GenderEnum Gender { get; set; }
    }
}
