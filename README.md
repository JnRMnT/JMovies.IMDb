# JMovies.IMDb
#### JMovies IMDb Data Provider Library
[![Build Status](https://dev.azure.com/jnrmnt/JMovies.IMDb/_apis/build/status/JnRMnT.JMovies.IMDb?branchName=master)](https://dev.azure.com/jnrmnt/JMovies.IMDb/_build/latest?definitionId=9&branchName=master) ![NuGet Downloads](https://img.shields.io/nuget/dt/JMovies.IMDb) ![NuGet Version](https://img.shields.io/nuget/v/JMovies.IMDb)

A IMDb data provider library written for .NET Standart platform that uses screen scraping to gather the data from IMDb on demand.

## How to Use?
- **Simple Usage:** After initializing the provider, simply by passing the movie ID, its details can be fetched. This will use a the most basic scraping and will provide only the information available on the production/person page.
```
IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
Movie movie = imdbDataProvider.GetMovie(232500);
```
- **Detailed Scraping:** GetMovie (also similar methods for other purposes) method optionally accepts a "DataFetchSettings" object. The scraper comes with a number of preset settings, by using "FullProductionDataFetchSettings"; it can be configured to visit all the related pages of the product and get the details from there and include in the movie object.
```
IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
Movie movie = imdbDataProvider.GetMovie(232500, new FullProductionDataFetchSettings());
```
- **Scraping in any other language:** As mentioned above, scrapers behaviour can be changed by providing a settings object. Any preset can be changed and simply by setting "PreferredCulture" as the culture name of the language which should be used while scraping, the scraper can be configured to use that language to get the results. Note that movie names etc. would be coming in that language.
```
IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
Entities.Movies.Movie movie = imdbDataProvider.GetMovie(232500, new BasicProductionDataFetchSettings
{
    PreferredCulture = "tr-TR"
}); // https://www.imdb.com/title/tt0232500/
```

## Under the hood
JMovies.IMDb uses .NET standart Http Web Requests to fetch the IMDB pages and parse them using HtmlAgilityPack+Fizzler.

## Maintenance
I will personaly maintain the project as much as i can but as it uses Screen Scraping it is possible that it will require some changes along the way, so every issue report and pull request is welcome!

## Code Documentation
Documentation website can be found under [JMovies.IMDb GitHub Pages](https://jnrmnt.github.io/JMovies.IMDb/html/d3/dcc/md__r_e_a_d_m_e.html).

## Warning
Please know that this library is not intended for bulk data scraping from IMDb and it is restricted by them. Also note that this library is not affliated with IMDB and you need to use it at your own risk/responsibility.
