namespace Pumpkin.Beer.Taste.Data;

using TimeZoneConverter;

public class User
{
    public string Id { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string WindowsTimeZoneId { get; set; } = null!;

    public string IanaTimeZoneId => TZConvert.WindowsToIana(this.WindowsTimeZoneId);
}
