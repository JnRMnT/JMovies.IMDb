using JMovies.IMDb.Entities.People;

namespace JMovies.IMDb.Entities.Movies
{
    public class Credit
    {
        public Person Person { get; set; }
        public CreditRoleType RoleType { get; set; }
    }
}
