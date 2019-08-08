using JMovies.IMDb.Entities.IMDB;
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