using JMovies.IMDb.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMovies.IMDb.Entities.People
{
    public class ProductionCredit
    {
        public Credit Credit { get; set; }
        public Production Production { get; set; }
    }
}
