using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.IMDB
{
    public class TVSeries: Movie
    {
        public int EndYear { get; set; }
        public override ProductionTypeEnum ProductonType => ProductionTypeEnum.TVSeries;
    }
}
