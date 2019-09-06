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
        public virtual ICollection<Credit> Credits { get; set; }

        /// <summary>
        /// Tag Lines of the movie
        /// </summary>
        [MaxLength(128)]
        [ForeignKey("ProductionID")]
        public virtual ICollection<TagLine> TagLines { get; set; }

        /// <summary>
        /// Keywords of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public virtual ICollection<Keyword> Keywords { get; set; }

        /// <summary>
        /// Genres of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public virtual ICollection<Genre> Genres { get; set; }

        /// <summary>
        /// Official Sites of the movie
        /// </summary>
        [MaxLength(512)]
        public ICollection<OfficialSite> OfficialSites { get; set; }

        /// <summary>
        /// Countries of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public virtual ICollection<ProductionCountry> Countries { get; set; }

        /// <summary>
        /// Languages of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public virtual ICollection<ProductionLanguage> Languages { get; set; }

        /// <summary>
        /// Release dates of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public virtual ICollection<ReleaseDate> ReleaseDates { get; set; }

        /// <summary>
        /// Alternative names of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public virtual ICollection<AKA> AKAs { get; set; }

        /// <summary>
        /// Filming Locations of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        [MaxLength(256)]
        public virtual ICollection<string> FilmingLocations { get; set; }

        /// <summary>
        /// Budget of the movie
        /// </summary>
        [MaxLength(256)]
        public Budget Budget { get; set; }

        /// <summary>
        /// Production Companies of the movie
        /// </summary>
        [ForeignKey("ProductionID")]
        public virtual ICollection<Company> ProductionCompanies { get; set; }

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
