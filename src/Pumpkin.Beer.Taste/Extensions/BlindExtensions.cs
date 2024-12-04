namespace Pumpkin.Beer.Taste.Eblindtensions;

using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;

public static class BlindExtensions
{
    public static bool IsOpen(
        this Blind blind,
        DateTimeOffset serverTimeNow,
        string usersTimeZoneId)
        => blind.HasEventStarted(serverTimeNow, usersTimeZoneId) && !blind.HasEventClosed(serverTimeNow, usersTimeZoneId);

    public static bool HasEventStarted(
        this Blind blind,
        DateTimeOffset serverTimeNow,
        string userTimeZoneId)
    {
        // This was already converted into UTC based on blind.StartedWindowsTimeZoneId when it was stored.
        var eventStartTimeUtc = blind.StartedUtc;

        // User in another time zone
        var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZoneId);
        var userCurrentTime = TimeZoneInfo.ConvertTimeFromUtc(serverTimeNow.DateTime, userTimeZone);

        // Convert UTC event start time to user's local time
        var eventStartTimeForUser = TimeZoneInfo.ConvertTimeFromUtc(eventStartTimeUtc, userTimeZone);

        return userCurrentTime >= eventStartTimeForUser;
    }

    public static bool HasEventClosed(
        this Blind blind,
        DateTimeOffset serverTimeNow,
        string userTimeZoneId)
    {
        // This was already converted into UTC based on blind.StartedWindowsTimeZoneId when it was stored.
        var eventStartTimeUtc = blind.ClosedUtc;

        // User in another time zone
        var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZoneId);
        var userCurrentTime = TimeZoneInfo.ConvertTimeFromUtc(serverTimeNow.DateTime, userTimeZone);

        // Convert UTC event start time to user's local time
        var eventCloseTimeForUser = TimeZoneInfo.ConvertTimeFromUtc(eventStartTimeUtc, userTimeZone);

        return userCurrentTime >= eventCloseTimeForUser;
    }
}
