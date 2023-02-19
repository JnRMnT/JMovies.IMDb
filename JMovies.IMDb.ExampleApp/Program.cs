using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Entities = JMovies.IMDb.Entities;

#region Basic Usage Example
IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
Entities.Movies.Movie movie = imdbDataProvider.GetMovie(232500, new BasicProductionDataFetchSettings());
Console.WriteLine($"Movie Title: {movie.Title}");
Console.WriteLine($"Original Title: {movie.OriginalTitle}");
Console.WriteLine($"Year: {movie.Year}");
Console.WriteLine($"Poster URL: {movie.Poster?.URL}");
Console.WriteLine($"Rating: {movie.Rating.Value}/{movie.Rating.RateCount} ratings");
Console.WriteLine("Press any key to continue...");
Console.ReadLine();
#endregion