using JM.Entities.Interfaces;
using JMovies.IMDb.Entities.Movies;

namespace JMovies.IMDb.Factories
{
    /// <summary>
    /// Factory class that is responsible for building productions based on the type
    /// </summary>
    public class ProductionFactory : IFactory<Production, ProductionTypeEnum>
    {
        /// <summary>
        ///  Build method of Production Factory that produces the given type of productions
        /// </summary>
        /// <param name="type">Type of production to be produced</param>
        /// <returns>New production instance</returns>
        public Production Build(ProductionTypeEnum type)
        {
            switch (type)
            {
                case ProductionTypeEnum.Movie:
                    return new Movie();
                case ProductionTypeEnum.TVSeries:
                    return new TVSeries();
                default:
                    return new Production();
            }
        }
    }
}
