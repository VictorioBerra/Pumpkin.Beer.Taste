namespace Pumpkin.Beer.Taste;

using System.Linq;
using Pumpkin.Beer.Taste.Data;
using SharpRepository.Repository.FetchStrategies;

public static class Strategies
{
    public static IFetchStrategy<Blind> IncludeItemsAndVotes() => new GenericFetchStrategy<Blind>().Include(x => x.BlindItems.First().BlindVotes);

    public static IFetchStrategy<BlindItem> IncludeVotes() => new GenericFetchStrategy<BlindItem>().Include(x => x.BlindVotes);

    public static IFetchStrategy<BlindItem> IncludeBlindAndVotes() => new GenericFetchStrategy<BlindItem>()
            .Include(x => x.BlindVotes)
            .Include(x => x.Blind);
}
