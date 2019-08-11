using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.People
{
    public class Person
    {
        public long IMDbID { get; set; }
        public string FullName { get; set; }
        public string PrimaryImage { get; set; }
        public CreditRoleType[] Roles { get; set; }

        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string BirthName { get; set; }

        public Length Height { get; set; }
        public string NickName { get; set; }
        public string MiniBiography { get; set; }

        public Image[] Photos { get; set; }

        public ProductionCredit[] KnownFor { get; set; }

        public GenderEnum Gender { get; set; }
    }
}
