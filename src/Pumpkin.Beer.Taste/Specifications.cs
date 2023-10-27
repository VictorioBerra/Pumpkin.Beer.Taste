namespace Pumpkin.Beer.Taste;

using System;
using System.Linq;
using Pumpkin.Beer.Taste.Data;
using SharpRepository.Repository.Specifications;

public static class Specifications
{
    public static Specification<Blind> GetOpenBlinds(DateTimeOffset now) => new Specification<Blind>(x => x.Started != null && x.Started < now && x.Closed != null && x.Closed > now);

    public static Specification<Blind> GetClosedBlinds(DateTimeOffset now) => new Specification<Blind>(x => x.Closed != null && x.Closed < now);

    public static Specification<Blind> GetOnlyMyBlinds(string userId) => new Specification<Blind>(x => x.CreatedByUserId == userId);

    public static Specification<Blind> GetBlindsWithNoVotes() =>
        // Give me the blind where there are no items, or
        // Give me all Blinds where there are items with no votes
        new Specification<Blind>(x => !x.BlindItems.Any() || x.BlindItems.Any(y => !y.BlindVotes.Any()));

    public static Specification<BlindItem> GetBlindsWithItemsWithNoVotesOfMine(string userId) =>
        // Give me all Blinds where there are items with no votes
        new Specification<BlindItem>(y => !y.BlindVotes.Any(z => z.CreatedByUserId == userId));

    public static Specification<BlindItem> GetBlindsWithItemsWithVotesOfMine(string userId) =>
        // Give me all Blinds where there are items with  votes
        new Specification<BlindItem>(y => y.BlindVotes.Any(z => z.CreatedByUserId == userId));
}
