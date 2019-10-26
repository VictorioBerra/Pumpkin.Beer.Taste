using Pumpkin.Beer.Taste.Data;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste
{
    public static class Strategies
    {
        public static IFetchStrategy<Blind> IncludeItemsAndVotes()
        {
            return new GenericFetchStrategy<Blind>().Include(x => x.BlindItems.First().BlindVotes);
        }

        public static IFetchStrategy<BlindItem> IncludeVotes()
        {
            return new GenericFetchStrategy<BlindItem>().Include(x => x.BlindVotes);
        }

        public static IFetchStrategy<BlindItem> IncludeBlindAndVotes()
        {
            return new GenericFetchStrategy<BlindItem>()
                .Include(x => x.BlindVotes)
                .Include(x => x.Blind);
        }
    }
}
