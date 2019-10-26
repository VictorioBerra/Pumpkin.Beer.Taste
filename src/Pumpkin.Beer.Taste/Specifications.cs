using Pumpkin.Beer.Taste.Data;
using SharpRepository.Repository.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste
{
    public static class Specifications
    {
        public static Specification<Blind> GetOpenBlinds(DateTimeOffset now)
        {
            return new Specification<Blind>(x => x.Started != null && x.Started < now && (x.Closed == null || x.Closed > now));
        }

        public static Specification<Blind> GetOnlyMyBlinds(string userId)
        {
            return new Specification<Blind>(x => x.CreatedByUserId == userId);
        }

        public static Specification<Blind> GetBlindsWithNoVotes()
        {
            // Give me the blind where there are no items, or
            // Give me all Blinds where there are items with no votes
            return new Specification<Blind>(x => !x.BlindItems.Any() || x.BlindItems.Any(y => !y.BlindVotes.Any()));
        }
    }
}
