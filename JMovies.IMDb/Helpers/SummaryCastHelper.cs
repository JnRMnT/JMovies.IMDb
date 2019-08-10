using System.Collections.Generic;
using HtmlAgilityPack;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using Fizzler.Systems.HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers
{
    public class SummaryCastHelper
    {
        internal static Credit[] GetCreditInfo(HtmlNode creditSummaryNode)
        {
            List<Credit> credits = new List<Credit>();
            string role = creditSummaryNode.QuerySelector("h4").InnerText.Prepare();
            CreditRoleType roleType = CreditRoleType.Undefined;
            if (IMDbConstants.DirectorsSummaryRegex.IsMatch(role))
            {
                roleType = CreditRoleType.Director;
            }
            else if (IMDbConstants.StarsSummaryRegex.IsMatch(role))
            {
                roleType = CreditRoleType.Acting;
            }
            else if (IMDbConstants.WritersSummaryRegex.IsMatch(role))
            {
                roleType = CreditRoleType.Writer;
            }
            else if (IMDbConstants.CreatorsSummaryRegex.IsMatch(role))
            {
                roleType = CreditRoleType.Creator;
            }

            foreach (HtmlNode creditNode in creditSummaryNode.QuerySelectorAll("a"))
            {
                Match personIDMatch = IMDbConstants.PersonIDURLMatcher.Match(creditNode.Attributes["href"].Value);
                if (personIDMatch.Success && personIDMatch.Groups.Count > 1)
                {
                    if (roleType == CreditRoleType.Acting)
                    {
                        //Ignore acting credits, they are covered from cast summary
                    }
                    else
                    {
                        Credit credit = new Credit
                        {
                            Person = new Person
                            {
                                IMDbID = personIDMatch.Groups[1].Value.ToLong(),
                                FullName = creditNode.InnerText.Prepare()
                            },
                            RoleType = roleType
                        };
                        credits.Add(credit);
                    }
                }
            }

            return credits.ToArray();
        }
    }
}
