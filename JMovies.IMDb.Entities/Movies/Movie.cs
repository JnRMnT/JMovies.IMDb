using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a movie
    /// </summary>
    [Table("Movie")]
    public class Movie: Production
    {
        /// <summary>
        /// Original Title of the movie
        /// </summary>
        [MaxLength(128)]
        public string OriginalTitle { get; set; }

        /// <summary>
        /// Plot Summary of the movie
        /// </summary>
        [MaxLength(512)]
        public string PlotSummary { get; set; }

        /// <summary>
        /// Story Line of the movie
        /// </summary>
        public string StoryLine { get; set; }

        /// <summary>
        /// Credits of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public ICollection<Credit> Credits { get; set; }

        /// <summary>
        /// Tag Lines of the movie
        /// </summary>
        [MaxLength(128)]
        public ICollection<string> TagLines { get; set; }

        /// <summary>
        /// Keywords of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public ICollection<Keyword> Keywords { get; set; }

        /// <summary>
        /// Genres of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public ICollection<Genre> Genres { get; set; }

        /// <summary>
        /// Official Sites of the movie
        /// </summary>
        [MaxLength(512)]
        public ICollection<OfficialSite> OfficialSites { get; set; }

        /// <summary>
        /// Countries of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public ICollection<Country> Countries { get; set; }

        /// <summary>
        /// Languages of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public ICollection<Language> Languages { get; set; }

        /// <summary>
        /// Release dates of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public ICollection<ReleaseDate> ReleaseDates { get; set; }

        /// <summary>
        /// Alternative names of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public ICollection<AKA> AKAs { get; set; }

        /// <summary>
        /// Filming Locations of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        [MaxLength(256)]
        public ICollection<string> FilmingLocations { get; set; }

        /// <summary>
        /// Budget of the movie
        /// </summary>
        [MaxLength(256)]
        public Budget Budget { get; set; }

        /// <summary>
        /// Production Companies of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public ICollection<Company> ProductionCompanies { get; set; }

        /// <summary>
        /// Length of the movie
        /// </summary>
        public TimeSpan Runtime { get; set; }
    }
}
