using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.IMDB
{
    public class Person
    {
        public long IMDbID { get; set; }
        public string FullName { get; set; }
        public string PrimaryImage { get; set; }
    }
}
