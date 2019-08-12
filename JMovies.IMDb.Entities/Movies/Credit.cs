using JMovies.IMDb.Entities.People;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a credit in a title
    /// </summary>
    public class Credit
    {
        /// <summary>
        /// Person related to the credit
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// Role of the person in the title
        /// </summary>
        public CreditRoleType RoleType { get; set; }
    }
}
