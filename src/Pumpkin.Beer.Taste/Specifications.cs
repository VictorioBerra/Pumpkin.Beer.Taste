namespace Pumpkin.Beer.Taste;

using System;
using System.Linq;
using Pumpkin.Beer.Taste.Data;
using SharpRepository.Repository.Specifications;

public static class Specifications
{
    public static Specification<Blind> GetOpenBlinds(DateTimeOffset now)
        => new(x => x.Started != null && x.Started < now && x.Closed != null && x.Closed > now);

    public static Specification<Blind> GetClosedBlinds(DateTimeOffset now) => new(x => x.Closed != null && x.Closed < now);

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
