using JM.Entities.Interfaces;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;

namespace JMovies.IMDb.Factories
{
    public class CreditFactory: IFactory<Credit, CreditRoleType>
    {
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
