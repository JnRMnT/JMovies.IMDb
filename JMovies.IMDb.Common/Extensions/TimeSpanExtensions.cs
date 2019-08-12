using JMovies.IMDb.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// Class responsible for providing Extenions for TimeSpans
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>
    /// Extension that converts HTML DateTime string into C# TimeSpan
    /// </summary>
    /// <param name="dateTimeString">HTML DateTime String</param>
    /// <returns>C# TimeSpan equivalent</returns>
    public static TimeSpan ToHtmlTimeSpan(this string dateTimeString)
    {
        TimeSpan timeSpan = TimeSpan.Zero;
        if (!string.IsNullOrEmpty(dateTimeString))
        {
            Match timeSpanMatch = GeneralRegexConstants.HtmlDatetimeTagRegex.Match(dateTimeString);
            if (timeSpanMatch.Success)
            {
                int days = 0;
                int hours = 0;
                int minutes = 0;
                int seconds = 0;
                for (int i = 1; i < timeSpanMatch.Groups.Count; i++)
                {
                    Group group = timeSpanMatch.Groups[i];
                    if (group.Value.EndsWith("D"))
                    {
                        days = group.Value.TrimEnd('D').ToInteger();
                    }
                    else if (group.Value.EndsWith("H"))
                    {
                        hours = group.Value.TrimEnd('H').ToInteger();
                    }
                    else if (group.Value.EndsWith("M"))
                    {
                        minutes = group.Value.TrimEnd('M').ToInteger();
                    }
                    else if (group.Value.EndsWith("S"))
                    {
                        seconds = group.Value.TrimEnd('S').ToInteger();
                    }
                }

                timeSpan = new TimeSpan(days, hours, minutes, seconds);
            }

            return timeSpan;
        }
        else
        {
            return timeSpan;
        }
    }
}