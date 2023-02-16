using JMovies.IMDb.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

    /// <summary>
    ///  Extension that converts human readable duration string into C# TimeSpan
    /// </summary>
    /// <param name="humanReadableDuration">The human readable string</param>
    /// <returns>C# TimeSpan equivalent</returns>
    public static TimeSpan HumanReadableToTimeSpan(this string humanReadableDuration)
    {
        Dictionary<string, int> units = new Dictionary<string, int>()
            {
                {@"(\d+)\s*(ms|mili[|s]|milisecon[|s])", 1 },
                {@"(\d+)\s*(s|sec|second[|s])", 1000 },
                {@"(\d+)\s*(m|min[|s])", 60000 },
                {@"(\d+)\s*(h|hour[|s])", 3600000 },
                {@"(\d+)\s*(d|day[|s])", 86400000 },
                {@"(\d+)\s*(w|week[|s])", 604800000 },
            };

        var timespan = new TimeSpan();

        foreach (var x in units)
        {
            var matches = Regex.Matches(humanReadableDuration, x.Key);
            foreach (Match match in matches)
            {
                var amount = System.Convert.ToInt32(match.Groups[1].Value);
                timespan = timespan.Add(TimeSpan.FromMilliseconds(x.Value * amount));
            }
        }

        return timespan;
    }
}