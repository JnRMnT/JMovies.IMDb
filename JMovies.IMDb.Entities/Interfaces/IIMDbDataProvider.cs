using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Interfaces
{
    public interface IIMDbDataProvider
    {
        Movie GetMovie(long id, bool fetchDetailedCast);
        Person GetPerson(long id);
    }
}