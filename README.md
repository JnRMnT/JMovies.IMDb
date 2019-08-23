# JMovies.IMDb
#### JMovies IMDb Data Provider Library
[![Build Status](https://dev.azure.com/jnrmnt/JMovies.IMDb/_apis/build/status/JnRMnT.JMovies.IMDb?branchName=master)](https://dev.azure.com/jnrmnt/JMovies.IMDb/_build/latest?definitionId=9&branchName=master) ![NuGet Downloads](https://img.shields.io/nuget/dt/JMovies.IMDb) ![NuGet Version](https://img.shields.io/nuget/v/JMovies.IMDb)

A IMDb data provider library written for .NET Standart platform that uses screen scraping to gather the data from IMDb on demand.


## Under the hood
JMovies.IMDb uses .NET standart Http Web Requests to fetch the IMDB pages and parse them using HtmlAgilityPack+Fizzler.

## Maintenance
I will personaly maintain the project as much as i can but as it uses Screen Scraping it is possible that it will require some changes along the way, so every issue report and pull request is welcome!

## Code Documentation
Documentation website can be found under [JMovies.IMDb GitHub Pages](https://jnrmnt.github.io/JMovies.IMDb/html/index.html).

## Warning
Please know that this library is not intended for bulk data scraping from IMDb and it is restricted by them. Also note that this library is not affliated with IMDB and you need to use it at your own risk/responsibility.
