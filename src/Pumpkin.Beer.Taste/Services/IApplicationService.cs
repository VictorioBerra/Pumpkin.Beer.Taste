namespace Pumpkin.Beer.Taste.Services;

using System.Security.Claims;
using Ardalis.Result;
using Pumpkin.Beer.Taste.Data;

public interface IApplicationService
{
    Result<Blind> AcceptInvite(ClaimsPrincipal user, string inviteCode);
}
