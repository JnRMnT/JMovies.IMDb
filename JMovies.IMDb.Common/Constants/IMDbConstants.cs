using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JMovies.IMDb.Common.Constants
{
    public class IMDbConstants
    {
        public static readonly int IMDbIDLength = 7;
        public static readonly string DefaultScrapingCulture = "en-US";
        public static readonly string BaseURL = "https://www.imdb.com/";
        public static readonly string MoviesPath = "title/";
        public static readonly string MovieIDPrefix = "tt";
        public static readonly string PersonsPath = "name/";
        public static readonly string PersonIDPrefix = "nm";
        public static readonly string CharactersPath = "characters/";
        public static readonly string KeywordsPath = "keyword/";
        public static readonly string CompanyIDPrefix = "co";
        public static readonly string CompaniesPath = "company/";
        public static readonly string FullCreditsPath = "fullcredits/";
        public static readonly string TVSeriesOgType = "video.tv_show";

        public static readonly string Star = "Star";
        public static readonly string Director = "Director";
        public static readonly string DirectedBy = "Directed By";
        public static readonly string Writer = "Writer";
        public static readonly string Creator = "Creator";
        public static readonly string Tagline = "Tagline";
        public static readonly string PlotKeyword = "Plot Keyword";
        public static readonly string Genre = "Genre";
        public static readonly string OfficialSite = "Official Site";
        public static readonly string CountryOfOriginFilterName = "country_of_origin";
        public static readonly string Language = "Language";
        public static readonly string PrimaryLanguageFilterName = "primary_language";
        public static readonly string ReleaseDate = "Release Date";
        public static readonly string AKA = "Also Known As";
        public static readonly string FilmingLocation = "Filming Location";
        public static readonly string LocationsFilterName = "locations";
        public static readonly string Budget = "Budget";
        public static readonly string ProductionCompany = "Production Co";
        public static readonly string Runtime = "Runtime";

        public static readonly Regex StarsSummaryRegex = new Regex(Star + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex DirectorsSummaryRegex = new Regex(Director + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex WritersSummaryRegex = new Regex(Writer + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex CreatorsSummaryRegex = new Regex(Creator + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex PersonIDURLMatcher = new Regex(Regex.Escape(PersonsPath) + PersonIDPrefix + @"(\d+)", RegexOptions.IgnoreCase);
        public static readonly Regex MovieYearRegex = new Regex(@"(.+)\((\d{4})[–-]?(\d{4})?\)\s*");
        public static readonly Regex CharacterRegex = new Regex(CharactersPath + PersonIDPrefix + @"(\d+).*?");
        public static readonly Regex TaglinesSummaryRegex = new Regex(Tagline + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex PlotKeywordsSummaryRegex = new Regex(PlotKeyword + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex KeywordLinkRegex = new Regex(Regex.Escape(KeywordsPath) + @"(.+)\?", RegexOptions.IgnoreCase);
        public static readonly Regex GenresSummaryRegex = new Regex(Genre + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex GenreLinkRegex = new Regex("title\\?genres=(.+?)[&\"]", RegexOptions.IgnoreCase);
        public static readonly Regex OfficialSitesHeaderRegex = new Regex(OfficialSite + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex CountriesHeaderRegex = new Regex("Countr[y]?(ies)?:", RegexOptions.IgnoreCase);
        public static readonly Regex CountryOfOriginRegex = new Regex(CountryOfOriginFilterName + "=(.+?)[&\"]", RegexOptions.IgnoreCase);
        public static readonly Regex LanguagesHeaderRegex = new Regex(Language + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex PrimaryLanguageRegex = new Regex(PrimaryLanguageFilterName + "=(.+?)[&\"]", RegexOptions.IgnoreCase);
        public static readonly Regex ReleaseDateHeaderRegex = new Regex(ReleaseDate + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex ReleaseDateRegex = new Regex(@"\s*(.+)\s*\((.+)\)", RegexOptions.IgnoreCase);
        public static readonly Regex AKAHeaderRegex = new Regex(AKA + ":", RegexOptions.IgnoreCase);
        public static readonly Regex FilmingLocationsHeaderRegex = new Regex(FilmingLocation + "[s]?:", RegexOptions.IgnoreCase);
        public static readonly Regex LocationsLinkRegex = new Regex(LocationsFilterName + "=.+\">(.+)<\\/a>", RegexOptions.IgnoreCase);
        public static readonly Regex BudgetHeaderRegex = new Regex(Budget + ":", RegexOptions.IgnoreCase);
        public static readonly Regex ProductionCompanyHeaderRegex = new Regex(ProductionCompany + ":", RegexOptions.IgnoreCase);
        public static readonly Regex ProductionCompanyLinkRegex = new Regex(CompaniesPath + CompanyIDPrefix + @"(\d+).*?");
        public static readonly Regex RuntimeHeaderRegex = new Regex(Runtime + ":", RegexOptions.IgnoreCase);
        public static readonly Regex CharacterEpisodeInfoRegex = new Regex(@"\n*\s*\(?(\d+)\s+episodes?,\s+(\d+)-?(\d*)\)?\s*\n*", RegexOptions.IgnoreCase);
    }
}
