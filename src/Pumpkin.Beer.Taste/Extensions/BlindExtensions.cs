namespace Pumpkin.Beer.Taste.Eblindtensions;

using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;

public static class BlindExtensions
{
    public static bool IsOpen(this Blind blind, DateTimeOffset now)
        => blind.Started != null && blind.Started < now && blind.Closed != null && blind.Closed > now;
}
