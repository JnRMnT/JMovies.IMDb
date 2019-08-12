using System;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a movie
    /// </summary>
    public class Movie: Production
    {
        /// <summary>
        /// Original Title of the movie
        /// </summary>
        public string OriginalTitle { get; set; }

        /// <summary>
        /// Plot Summary of the movie
        /// </summary>
        public string PlotSummary { get; set; }

        /// <summary>
        /// Story Line of the movie
        /// </summary>
        public string StoryLine { get; set; }

        /// <summary>
        /// Credits of the movie
        /// </summary>
        public Credit[] Credits { get; set; }

        /// <summary>
        /// Tag Lines of the movie
        /// </summary>
        public string[] TagLines { get; set; }

        /// <summary>
        /// Keywords of the movie
        /// </summary>
        public Keyword[] Keywords { get; set; }

        /// <summary>
        /// Genres of the movie
        /// </summary>
        public Genre[] Genres { get; set; }

        /// <summary>
        /// Official Sites of the movie
        /// </summary>
        public OfficialSite[] OfficialSites { get; set; }

        /// <summary>
        /// Countries of the movie
        /// </summary>
        public Country[] Countries { get; set; }

        /// <summary>
        /// Languages of the movie
        /// </summary>
        public Language[] Languages { get; set; }

        /// <summary>
        /// Release dates of the movie
        /// </summary>
        public ReleaseDate[] ReleaseDates { get; set; }

        /// <summary>
        /// Alternative names of the movie
        /// </summary>
        public AKA[] AKAs { get; set; }

        /// <summary>
        /// Filming Locations of the movie
        /// </summary>
        public string[] FilmingLocations { get; set; }

        /// <summary>
        /// Budget of the movie
        /// </summary>
        public Budget Budget { get; set; }

        /// <summary>
        /// Production Companies of the movie
        /// </summary>
        public Company[] ProductionCompanies { get; set; }

        /// <summary>
        /// Length of the movie
        /// </summary>
        public TimeSpan Runtime { get; set; }

        /// <summary>
        /// Type of the production
        /// </summary>
        public override ProductionTypeEnum ProductionType => ProductionTypeEnum.Movie;
    }
}
