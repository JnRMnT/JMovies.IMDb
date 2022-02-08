using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a character in a title
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Primary key of a character
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public long ID { get; set; }


        /// <summary>
        /// Underlining Character Type
        /// </summary>
        [Required]
        [Column(Order = 2)]
        [MaxLength(2)]
        public virtual CharacterTypeEnum CharacterType { get; set; }

        /// <summary>
        /// IMDb ID
        /// </summary>
        [Column(Order = 1)]
        public long? IMDbID { get; set; }


        /// <summary>
        /// Name of the character
        /// </summary>
        [Required]
        [MaxLength(128)]
        [Column(Order = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Reference ID of related credit
        /// </summary>
        public long CreditID { get; set; }
    }
}
