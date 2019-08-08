using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.IMDB
{
    public class Movie
    {
        public long IMDbID { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string PlotSummary { get; set; }
        public string StoryLine { get; set; }
        public Credit[] Credits { get; set; }
        public int Year { get; set; }
        public string[] TagLines { get; set; }
        public Keyword[] Keywords { get; set; }
        public Genre[] Genres { get; set; }
        public OfficialSite[] OfficialSites { get; set; }
        public Country[] Countries { get; set; }
        public Language[] Languages { get; set; }
        public ReleaseDate[] ReleaseDates { get; set; }
        public AKA[] AKAs { get; set; }
        public string[] FilmingLocations { get; set; }
        public Budget Budget { get; set; }
        public Company[] ProductionCompanies { get; set; }
        public TimeSpan Runtime { get; set; }
        public virtual ProductionTypeEnum ProductonType
        {
            get
            {
                return ProductionTypeEnum.Movie;
            }
        }
    }
}
