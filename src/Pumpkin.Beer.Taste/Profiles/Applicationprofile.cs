using AutoMapper;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Profiles
{
    public class Applicationprofile : Profile
    {

        public Applicationprofile()
        {
            CreateMap<Blind, BlindDto>().ReverseMap();

            CreateMap<BlindItem, BlindItemDto>().ReverseMap();

            CreateMap<BlindVoteDto, BlindVote>().ReverseMap();

            CreateMap<BlindVote, BlindVoteDto>()
                .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.User.UserName));

        }
    }
}
