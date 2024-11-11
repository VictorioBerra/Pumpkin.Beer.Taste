namespace Pumpkin.Beer.Taste.Extensions;

using System.Security.Claims;

public static class UserExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId ?? throw new InvalidOperationException("Unable to get user ID.");
    }

    public static string GetUsername(this ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.Email);
        return userId ?? throw new InvalidOperationException("Unable to get user ID.");
    }
}
