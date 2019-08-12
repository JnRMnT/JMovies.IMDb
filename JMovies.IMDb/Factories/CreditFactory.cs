using JM.Entities.Interfaces;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;

namespace JMovies.IMDb.Factories
{
    /// <summary>
    /// Factory class that is responsible for building credits based on the role type
    /// </summary>
    public class CreditFactory: IFactory<Credit, CreditRoleType>
    {
        /// <summary>
        /// Build method of Credit Factory that produces the given type of credits
        /// </summary>
        /// <param name="type">Type of credits to be produced</param>
        /// <returns>New credit instance</returns>
        public Credit Build(CreditRoleType type)
        {
            switch (type)
            {
                case CreditRoleType.Actor:
                case CreditRoleType.Actress:
                case CreditRoleType.Acting:
                    return new ActingCredit();
                default:
                    return new Credit();
            }
        }
    }
}
