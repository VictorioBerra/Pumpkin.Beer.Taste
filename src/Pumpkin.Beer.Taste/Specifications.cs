namespace Pumpkin.Beer.Taste;

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pumpkin.Beer.Taste.Data;
using SharpRepository.Repository.Specifications;

public static class Specifications
{
    public static Specification<Blind> GetOpenBlinds(DateTime userCurrentTime, string timeZoneId)
        => new(x => EF.Functions.AtTimeZone(x.StartedUtc, timeZoneId) >= userCurrentTime);

    public static Specification<Blind> GetClosedBlinds(DateTime userCurrentTime, string timeZoneId)
        => new(x => EF.Functions.AtTimeZone(x.ClosedUtc, timeZoneId) >= userCurrentTime);

    public static Specification<Blind> GetOwnedBlinds(string userId) => new(x => x.CreatedByUserId == userId);

    public static Specification<Blind> GetMemberOfBlinds(string userId) => new(x => x.UserInvites.Any(y => y.CreatedByUserId == userId));

    public static Specification<Blind> GetBlindsWithNoVotes() =>

        // Give me the blind where there are no items, or
        // Give me all Blinds where there are items with no votes
        new(x => x.BlindItems.Count == 0 || x.BlindItems.Any(y => y.BlindVotes.Count == 0));

    public static Specification<BlindItem> GetBlindsWithItemsWithNoVotesOfMine(string userId) =>

        // Give me all Blinds where there are items with no votes
        new(y => !y.BlindVotes.Any(z => z.CreatedByUserId == userId));

    public static Specification<BlindItem> GetBlindsWithItemsWithVotesOfMine(string userId) =>

        // Give me all Blinds where there are items with  votes
        new(y => y.BlindVotes.Any(z => z.CreatedByUserId == userId));
}
