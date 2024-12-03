namespace Pumpkin.Beer.Taste.Services;

using System.Security.Claims;
using Ardalis.Result;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Eblindtensions;
using Pumpkin.Beer.Taste.Extensions;
using SharpRepository.Repository;

public class ApplicationService(
    ILogger<ApplicationService> logger,
    TimeProvider timeProvider,
    IRepository<Blind, int> blindRepository,
    IRepository<User, string> userRepository,
    IRepository<UserInvite, int> inviteRepository) : IApplicationService
{
    public Result<Blind> AcceptInvite(ClaimsPrincipal user, string inviteCode)
    {
        var userId = user.GetUserId();
        var dbUser = userRepository.Get(userId);
        var now = timeProvider.GetLocalNow();

        var blindForInvite = blindRepository.Find(blind => blind.InviteCode == inviteCode);

        if (blindForInvite is null)
        {
            logger.LogWarning("User {UserId} attempted to join a blind with invalid invite code {InviteCode}", userId, inviteCode);
            return Result.Error("Invalid invite code.");
        }

        var existingLink = inviteRepository.Find(blind => blind.CreatedByUserId == userId && blind.BlindId == blindForInvite.Id);
        if (existingLink is null)
        {
            var invite = new UserInvite
            {
                BlindId = blindForInvite.Id,
                CreatedByUserId = userId,
            };
            inviteRepository.Add(invite);

            logger.LogInformation("User {UserId} joined blind {BlindId} with invite code {InviteCode}", userId, blindForInvite.Id, inviteCode);
        }

        if (blindForInvite.HasEventStarted(now, dbUser.WindowsTimeZoneId))
        {
            return Result.Success(blindForInvite);
        }
        else
        {
            return Result.Success();
        }
    }
}
