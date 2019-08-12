using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.Movies;
using System;
using System.Collections.Generic;
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
        /// IMDb ID of the person
        /// </summary>
        public long IMDbID { get; set; }

        /// <summary>
        /// Full Name of the person
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Main image of the person
        /// </summary>
        public string PrimaryImage { get; set; }

        /// <summary>
        /// Roles of the person
        /// </summary>
        public CreditRoleType[] Roles { get; set; }

        /// <summary>
        /// Birth Date of the person
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Birth Place of the person
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// Birth Place of the person
        /// </summary>
        public string BirthName { get; set; }

        /// <summary>
        /// Height of the person
        /// </summary>
        public Length Height { get; set; }

        /// <summary>
        /// NickName of the person
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Mini Biography of the person
        /// </summary>
        public string MiniBiography { get; set; }

        /// <summary>
        /// Available Photos of the person
        /// </summary>
        public Image[] Photos { get; set; }

        /// <summary>
        /// Known credits of the person
        /// </summary>

        public ProductionCredit[] KnownFor { get; set; }

        /// <summary>
        /// Gender of the person
        /// </summary>
        public GenderEnum Gender { get; set; }
    }
}
