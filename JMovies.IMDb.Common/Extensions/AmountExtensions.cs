using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Common;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class AmountExtensions
{
    public static Amount ToAmount(this string amountString)
    {
        Match match = GeneralRegexConstants.AmountStringRegex.Match(amountString);
        if (match.Success)
        {
            Amount amount = new Amount();
            amount.Value = match.Groups[2].Value.ToMoneyDecimal();
            amount.Currency = (match.Groups[3].Success && !string.IsNullOrEmpty(match.Groups[3].Value)) ? match.Groups[3].Value : match.Groups[1].Value;
            return amount;
        }
        else
        {
            return null;
        }
    }
}
