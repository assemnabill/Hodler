using Corz.Extensions.DateTime;

namespace Hodler.Domain.Shared;

public static class DateOnlyExtensions
{
    public static long ToUnixTimeSeconds(this DateOnly date) => date.ToDateTimeOffset().ToUnixTimeSeconds();

    public static DateOnly FromUnixTimeSeconds(this long unixTimeSeconds)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds);
        return DateOnly.FromDateTime(dateTimeOffset.DateTime);
    }

    public static DateOnly FromUnixTimeMilliseconds(this long unixTimeMilliseconds)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds);
        return DateOnly.FromDateTime(dateTimeOffset.DateTime);
    }

    public static long ToUnixTimeMilliseconds(this DateOnly date)
    {
        var dateTimeOffset = date.ToDateTimeOffset();
        return dateTimeOffset.ToUnixTimeMilliseconds();
    }
}