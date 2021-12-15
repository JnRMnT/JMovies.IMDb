using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JMovies.IMDb.Common.Constants
{
    /// <summary>
    /// Class that holds constant definitions that are related to IMDb
    /// </summary>
    public class IMDbConstants
    {
        /// <summary>
        /// Max Retry Count for HTTP Calls
        /// </summary>
        public static readonly int HttpMaxRetryCount = 5;

        /// <summary>
        /// Sleep duration in ms to be waited if an exception occurs during Http calls
        /// </summary>
        public static readonly int HttpRetrySleepTime = 1000;

        /// <summary>
        /// Length of the Extended IMDb IDs
        /// </summary>
        public static readonly int IMDbIDExtendedLength = 8;

        /// <summary>
        /// Length of the IMDb IDs
        /// </summary>
        public static readonly int IMDbIDLength = 7;

        /// <summary>
        /// Default culture to be used while screen scraping
        /// </summary>
        public static readonly string DefaultScrapingCulture = "en-US";

        /// <summary>
        /// Base URL of the IMDb website
        /// </summary>
        public static readonly string BaseURL = "https://www.imdb.com/";

        /// <summary>
        /// Path of the movies page
        /// </summary>
        public static readonly string MoviesPath = "title/";

        /// <summary>
        /// Prefix of the movie IDs
        /// </summary>
        public static readonly string MovieIDPrefix = "tt";

        /// <summary>
        /// Path of the person page
        /// </summary>
        public static readonly string PersonsPath = "name/";

        /// <summary>
        /// Prefix of the person IDs
        /// </summary>
        public static readonly string PersonIDPrefix = "nm";

        /// <summary>
        /// Path of the characters page
        /// </summary>
        public static readonly string CharactersPath = "characters/";

        /// <summary>
        /// Path of the keywords page
        /// </summary>
        public static readonly string KeywordsPath = "keyword";

        /// <summary>
        /// Prefix of Company IDs
        /// </summary>
        public static readonly string CompanyIDPrefix = "co";

        /// <summary>
        /// Path of the companies page
        /// </summary>
        public static readonly string CompaniesPath = "company/";

        /// <summary>
        /// Path of the Full Credits page
        /// </summary>
        public static readonly string FullCreditsPath = "fullcredits/";

        /// <summary>
        /// Path of the Release Info page
        /// </summary>
        public static readonly string ReleaseInfoPath = "releaseinfo/";

        /// <summary>
        /// Path of the Photo Gallery Page
        /// </summary>
        public static readonly string PhotoGalleryPath = "mediaindex/";

        /// <summary>
        /// Type indicator for TV Series
        /// </summary>
        public static readonly string TVSeriesOgType = "video.tv_show";

        /// <summary>
        /// Path of Bio page
        /// </summary>
        public static readonly string BioPagePath = "bio/";

        /// <summary>
        /// Text of Star title
        /// </summary>
        public static readonly string Star = "Star";

        /// <summary>
        /// Text of Director title
        /// </summary>
        public static readonly string Director = "Director";

        /// <summary>
        /// Text of Directed by title
        /// </summary>
        public static readonly string DirectedBy = "Directed By";

        /// <summary>
        /// Text of Writer title
        /// </summary>
        public static readonly string Writer = "Writer";

        /// <summary>
        /// Text of Creator title
        /// </summary>
        public static readonly string Creator = "Creator";

        /// <summary>
        /// Text of Taglines title
        /// </summary>
        public static readonly string Tagline = "Tagline";

        /// <summary>
        /// Text of Genre title
        /// </summary>
        public static readonly string Genre = "Genre";

        /// <summary>
        /// Text of Official Site title
        /// </summary>
        public static readonly string OfficialSite = "Official Site";

        /// <summary>
        /// Filter name of "Country Of Origin"
        /// </summary>
        public static readonly string CountryOfOriginFilterName = "country_of_origin";

        /// <summary>
        /// Text of Language title
        /// </summary>
        public static readonly string Language = "Language";

        /// <summary>
        /// Filter name of "Primary Language"
        /// </summary>
        public static readonly string PrimaryLanguageFilterName = "primary_language";

        /// <summary>
        /// Text of Release Date title
        /// </summary>
        public static readonly string ReleaseDate = "Release Date";

        /// <summary>
        /// Text of AKAs title
        /// </summary>
        public static readonly string AKA = "Also Known As";

        /// <summary>
        /// Text of Filming Location title
        /// </summary>
        public static readonly string FilmingLocation = "Filming Location";

        /// <summary>
        /// Filter name of "Locations"
        /// </summary>
        public static readonly string LocationsFilterName = "locations";

        /// <summary>
        /// Text of Budget title
        /// </summary>
        public static readonly string Budget = "Budget";

        /// <summary>
        /// Text of Production Company title
        /// </summary>
        public static readonly string ProductionCompany = "Production Co";

        /// <summary>
        /// Text of Runtime title
        /// </summary>
        public static readonly string Runtime = "Runtime";

        #region Biography Page Table Labels
        /// <summary>
        /// Bio Page - Overview Section - Birth Name Title
        /// </summary>
        public const string BioOverviewBirthName = "Birth Name";

        /// <summary>
        /// Bio Page - Overview Section - Birth Information Title
        /// </summary>
        public const string BioOverviewBirthInfo = "Born";

        /// <summary>
        /// Bio Page - Overview Section - Height Title
        /// </summary>
        public const string BioOverviewHeight = "Height";

        /// <summary>
        /// Bio Page - Overview Section - NickName Title
        /// </summary>
        public const string BioOverviewNickName = "Nickname";
        #endregion
        #region Filmography Categories
        /// <summary>
        /// Actor Filmography Category Name
        /// </summary>
        public static readonly string ActorCategoryName = "actor";
        /// <summary>
        /// Actress Filmography Category Name
        /// </summary>
        public static readonly string ActressCategoryName = "actress";

        /// <summary>
        /// Empty text to be checked to determine if there is no plot
        /// </summary>
        public static readonly string EmptyPlotText = "Add a plot";
        #endregion
        #region IMDB specific Regexes
        /// <summary>
        /// Regex that matches the title
        /// </summary>
        public static readonly Regex ProductionTitleMatcherRegex = new Regex(@"\/title\/tt(\d+)\/");
        /// <summary>
        /// Regex that matches the stars section in Movie Summary
        /// </summary>
        public static readonly Regex StarsSummaryRegex = new Regex(Star + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the directors section in Movie Summary
        /// </summary>
        public static readonly Regex DirectorsSummaryRegex = new Regex(Director + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the writers section in Movie Summary
        /// </summary>
        public static readonly Regex WritersSummaryRegex = new Regex(Writer + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the creators section in Movie Summary
        /// </summary>
        public static readonly Regex CreatorsSummaryRegex = new Regex(Creator + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the Person ID from URL
        /// </summary>
        public static readonly Regex PersonIDURLMatcher = new Regex(Regex.Escape(PersonsPath) + PersonIDPrefix + @"(\d+)", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the movie year in the Movie page
        /// </summary>
        public static readonly Regex MovieYearRegex = new Regex(@"\s*(\d{4})[–-]?(\d{4})?\s*");
        /// <summary>
        /// Regex that matches the character IMDb ID
        /// </summary>
        public static readonly Regex CharacterRegex = new Regex(CharactersPath + PersonIDPrefix + @"(\d+).*?");
        /// <summary>
        /// Regex that matches the tag lines section in Movie Summary
        /// </summary>
        public static readonly Regex TaglinesSummaryRegex = new Regex(Tagline + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the keywords section in Movie Summary
        /// </summary>
        public static readonly Regex KeywordLinkRegex = new Regex(KeywordsPath + @"[\/?]{1,2}keywords=(.+)&", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the genres section in Movie Summary
        /// </summary>
        public static readonly Regex GenresSummaryRegex = new Regex(Genre + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches a specific Genre Link
        /// </summary>
        public static readonly Regex GenreLinkRegex = new Regex(@"title[/\\]\?genres=(.+?)[&\\""]", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the Official Sites section in Movie Summary
        /// </summary>
        public static readonly Regex OfficialSitesHeaderRegex = new Regex(OfficialSite + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the countries section in Movie Summary
        /// </summary>
        public static readonly Regex CountriesHeaderRegex = new Regex("Countr[y]?(ies)?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the Country Of Origin section in Movie Summary
        /// </summary>
        public static readonly Regex CountryOfOriginRegex = new Regex(CountryOfOriginFilterName + "=(.+?)[&\"]", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the Languages section in Movie Summary
        /// </summary>
        public static readonly Regex LanguagesHeaderRegex = new Regex(Language + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the Primary Language section in Movie Summary
        /// </summary>
        public static readonly Regex PrimaryLanguageRegex = new Regex(PrimaryLanguageFilterName + "=(.+?)[&\"]", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the Release Date section in Movie Summary
        /// </summary>
        public static readonly Regex ReleaseDateHeaderRegex = new Regex(ReleaseDate + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the Country Identifier of a Release Date
        /// </summary>
        public static readonly Regex ReleaseDateCountryIdentifierRegex = new Regex(@"region=(.+?)&", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the AKAs section in Movie Summary
        /// </summary>
        public static readonly Regex AKAHeaderRegex = new Regex(AKA + ":?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the filming locations section in Movie Summary
        /// </summary>
        public static readonly Regex FilmingLocationsHeaderRegex = new Regex(FilmingLocation + "[s]?:?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the locations section in Movie Summary
        /// </summary>
        public static readonly Regex LocationsLinkRegex = new Regex(LocationsFilterName + "=.+\">(.+)<\\/a>", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the budget section in Movie Summary
        /// </summary>
        public static readonly Regex BudgetHeaderRegex = new Regex(Budget + ":?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the production company section in Movie Summary
        /// </summary>
        public static readonly Regex ProductionCompanyHeaderRegex = new Regex(ProductionCompany + ":?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the production company link
        /// </summary>
        public static readonly Regex ProductionCompanyLinkRegex = new Regex(CompaniesPath + CompanyIDPrefix + @"(\d+).*?");
        /// <summary>
        /// Regex that matches the runtime length section in Movie Summary
        /// </summary>
        public static readonly Regex RuntimeHeaderRegex = new Regex(Runtime + ":?", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the epsidoe info of a character
        /// </summary>
        public static readonly Regex CharacterEpisodeInfoRegex = new Regex(@"\n*\s*\(?(\d+)\s+eps\s+•\s+(\d+)[\-–]?(\d*)", RegexOptions.IgnoreCase);
        /// <summary>
        /// Regex that matches the height of a person in Bio page
        /// </summary>
        public static readonly Regex BioHeightRegex = new Regex(@"\((\d\.\d{1,2}).*m\)");
        /// <summary>
        /// Resource indipendent IMDb ID matcher regex
        /// </summary>
        public static readonly Regex IMDBIDRegex = new Regex("(" + MovieIDPrefix + "|" + PersonIDPrefix + "|" + CompanyIDPrefix + @")(\d+)");
        /// <summary>
        /// Regex that matches the credit year
        /// </summary>
        public static readonly Regex CreditYearRegex = new Regex(@"(\d{4})(\/\w)?[–-]?(\d{4})?(\/\w)?");

        /// <summary>
        /// Regex that matches the rating out of context JSON
        /// </summary>
        public static readonly Regex RatingJSONLDMatcher = new Regex(@"{""@type"":""AggregateRating"".+?}");

        /// <summary>
        /// Regex that matches the rating count out of rating JSON
        /// </summary>
        public static readonly Regex RatingCountMatcher = new Regex(@"""ratingCount""\s*:\s*(\d+)");

        /// <summary>
        /// Regex that matches the rating value out of rating JSON
        /// </summary>
        public static readonly Regex RatingValueMatcher = new Regex(@"""ratingValue"":([\d\.,]*)");
        #endregion
    }
}
