using System.Globalization;
using GenDateTools;
using GenotekToGedcom.Models.Genotek;
using Unidecode.NET;

namespace GenotekToGedcom.Utils;

public static class StringUtils
{
    public static string? ToGedcomDate(this Birthdate[]? dates)
    {
        if (dates == null || !dates.Any())
        {
            return null;
        }
        else
        {
            return dates.First().ToGedcomDate();
        }
    }
    
    public static string? ToGedcomDate(this Birthdate? date)
    {
        if (date == null 
            || (!date.Year.HasValue && !date.Month.HasValue && !date.Day.HasValue))
        {
            return null;
        }

        var year = date.Year?.ToString(CultureInfo.InvariantCulture) ?? "0000";
        var month = date.Month?.ToString(CultureInfo.InvariantCulture) ?? "00";
        var day = date.Day?.ToString(CultureInfo.InvariantCulture) ?? "00";

        return new DatePart($"{year}{month}{day}")
            .ToString(CultureInfo.InvariantCulture);
    }
    
    public static string ToSingleString(IEnumerable<string>? strings, bool transliterate = false)
    {
        var result = string.Join(' ', strings ?? Array.Empty<string>());

        if (string.IsNullOrWhiteSpace(result))
        {
            return result;
        }
        
        if (transliterate)
        {
            result = result.Unidecode();
        }

        return result;
    }
}
