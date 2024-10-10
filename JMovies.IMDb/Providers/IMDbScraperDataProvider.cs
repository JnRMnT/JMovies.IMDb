﻿using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JM.Entities.Framework;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.Settings;
using JMovies.IMDb.Helpers;
using JMovies.IMDb.Helpers.Movies;
using JMovies.IMDb.Helpers.People;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace JMovies.IMDb.Providers
{
    /// <summary>
    /// Data provider that fetches the data real-time on IMDb website using Screen Scraping and returns it as per defined in IIMDbDataProvider.
    /// </summary>
    public class IMDbScraperDataProvider : IIMDbDataProvider
    {
        /// <inheritdoc/>
        public Task<Production> GetProductionAsync(long id, ProductionDataFetchSettings settings)
        {
            return Task.Run<Production>(() =>
            {
                string oldthreadCulture = Thread.CurrentThread.CurrentCulture.Name;
                string oldthreadUICulture = Thread.CurrentThread.CurrentUICulture.Name;
                try
                {
                    if (settings.PreferredCulture != oldthreadCulture && !string.IsNullOrEmpty(settings.PreferredCulture))
                    {
                        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(settings.PreferredCulture);
                        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                    }
                    if (id == default(long))
                    {
                        throw new JMException("IMDbIDEmpty");
                    }
                    else if (settings == null)
                    {
                        throw new JMException("SettingsEmpty");
                    }

                    Movie movie = new Movie();
                    string url = IMDbConstants.BaseURL + IMDbConstants.MoviesPath + IMDbConstants.MovieIDPrefix + IMDBIDHelper.GetPaddedIMDBId(id);
                    HtmlDocument htmlDocument = HtmlHelper.GetNewHtmlDocument();
                    WebRequest webRequest = HttpHelper.InitializeWebRequest(url + "/", settings);
                    using (Stream stream = HttpHelper.GetResponseStream(webRequest, settings))
                    {
                        htmlDocument.Load(stream, Encoding.UTF8);
                    }
                    HtmlNode documentNode = htmlDocument.DocumentNode;

                    //Parse and verify IMDb ID Meta Tag
                    string foundID = IMDbConstants.ProductionTitleMatcherRegex.Match(documentNode.InnerHtml)?.Groups?[1]?.Value;
                    if (!string.IsNullOrEmpty(foundID))
                    {
                        movie.IMDbID = foundID.ToLong();
                    }
                    else
                    {
                        return null;
                    }

                    MoviePageDetailsHelper.Parse(this, ref movie, documentNode, url, settings);

                    return movie;
                }
                finally
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(oldthreadCulture);
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(oldthreadUICulture);
                }
            });
        }

        /// <inheritdoc/>
        public async Task<Person> GetPersonAsync(long id, PersonDataFetchSettings settings)
        {
            Person person = new Person();
            return await AsyncGetPerson(id, person, settings);
        }

        /// <inheritdoc/>
        public Task<Person> AsyncGetPerson(long id, Person person, PersonDataFetchSettings settings)
        {
            return Task.Run(() =>
            {
                string oldthreadCulture = Thread.CurrentThread.CurrentCulture.Name;
                string oldthreadUICulture = Thread.CurrentThread.CurrentUICulture.Name;
                try
                {
                    if (settings.PreferredCulture != oldthreadCulture && !string.IsNullOrEmpty(settings.PreferredCulture))
                    {
                        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(settings.PreferredCulture);
                        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                    }
                    if (id == default(long))
                    {
                        throw new JMException("IMDbIDEmpty");
                    }

                    string url = IMDbConstants.BaseURL + IMDbConstants.PersonsPath + IMDbConstants.PersonIDPrefix + IMDBIDHelper.GetPaddedIMDBId(id);
                    HtmlDocument htmlDocument = HtmlHelper.GetNewHtmlDocument();

                    WebRequest webRequest = HttpHelper.InitializeWebRequest(url, settings);
                    using (Stream stream = HttpHelper.GetResponseStream(webRequest, settings))
                    {
                        htmlDocument.Load(stream, Encoding.UTF8);
                    }
                    HtmlNode documentNode = htmlDocument.DocumentNode;

                    //Parse and verify IMDb ID Meta Tag
                    HtmlNode idMetaTag = documentNode.QuerySelector("meta[property='imdb:pageConst']");
                    if (idMetaTag != null)
                    {
                        person.IMDbID = Regex.Replace(idMetaTag.Attributes["content"].Value, IMDbConstants.PersonIDPrefix, string.Empty).ToLong();
                    }
                    else
                    {
                        return null;
                    }

                    PersonPageHelper.Parse(person, documentNode, settings);

                    #region Parse Photo Gallery Page
                    if (settings.MediaImagesFetchCount > 0)
                    {
                        Helpers.People.PhotoGalleryPageHelper.Parse(person, url, settings);
                    }
                    #endregion
                    return person;
                }
                finally
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(oldthreadCulture);
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(oldthreadUICulture);
                }
            });
        }

        /// <inheritdoc/>
        public async Task<Movie> GetMovieAsync(long id, ProductionDataFetchSettings settings)
        {
            return await GetProductionAsync(id, settings) as Movie;
        }

        /// <inheritdoc/>
        public async Task<TVSeries> GetTvSeriesAsync(long id, ProductionDataFetchSettings settings)
        {
            return await GetProductionAsync(id, settings) as TVSeries;
        }

        /// <inheritdoc/>
        public async Task<Production> GetProductionAsync(long id)
        {
            return await GetProductionAsync(id, new ProductionDataFetchSettings());
        }

        /// <inheritdoc/>
        public async Task<Movie> GetMovieAsync(long id)
        {
            return await GetMovieAsync(id, new ProductionDataFetchSettings());
        }

        /// <inheritdoc/>
        public Movie GetMovie(long id)
        {
            return GetMovie(id, new ProductionDataFetchSettings());
        }

        /// <inheritdoc/>
        public async Task<TVSeries> GetTvSeriesAsync(long id)
        {
            return await GetTvSeriesAsync(id, new ProductionDataFetchSettings());
        }

        /// <inheritdoc/>
        public async Task<Person> GetPersonAsync(long id)
        {
            return await GetPersonAsync(id, new PersonDataFetchSettings());
        }

        /// <inheritdoc/>
        public Production GetProduction(long id)
        {
            return GetProduction(id, new ProductionDataFetchSettings());
        }

        /// <inheritdoc/>
        TVSeries IIMDbDataProvider.GetTvSeries(long id)
        {
            return GetTvSeries(id, new ProductionDataFetchSettings());
        }

        /// <inheritdoc/>
        public Production GetProduction(long id, ProductionDataFetchSettings settings)
        {
            Task<Production> task = GetProductionAsync(id, settings);
            task.Wait();
            return task.Result;
        }

        /// <inheritdoc/>
        public Movie GetMovie(long id, ProductionDataFetchSettings settings)
        {
            Task<Production> task = GetProductionAsync(id, settings);
            task.Wait();
            return task.Result as Movie;
        }

        /// <inheritdoc/>
        public TVSeries GetTvSeries(long id, ProductionDataFetchSettings settings)
        {
            Task<Production> task = GetProductionAsync(id, settings);
            task.Wait();
            return task.Result as TVSeries;
        }

        /// <inheritdoc/>
        public Person GetPerson(long id, PersonDataFetchSettings settings)
        {
            Person person = new Person();
            Task<Person> task = AsyncGetPerson(id, person, settings);
            task.Wait();
            return task.Result;
        }

        /// <inheritdoc/>
        public Person GetPerson(long id)
        {
            return GetPerson(id, new PersonDataFetchSettings());
        }
    }
}