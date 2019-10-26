using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
            CreateMap<BlindDto, Blind>();

            CreateMap<Blind, BlindDto>()
                .ForMember(dest => dest.CreatedByUsername, opts => opts.MapFrom(src => src.CreatedByUser.UserName));

            CreateMap<BlindItem, BlindItemDto>()
                .ForMember(dest => dest.Letter, opts => opts.Ignore());
            CreateMap<BlindItemDto, BlindItem>();

            CreateMap<BlindVoteDto, BlindVote>();
            CreateMap<BlindVote, BlindVoteDto>()
                .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.CreatedByUser.UserName));


            CreateMap<BlindItem, BlindItemScoresDto>()
                .ForMember(dest => dest.AmountOfVotes, opts => opts.MapFrom(src => src.BlindVotes.Count()))
                .ForMember(dest => dest.TotalScore, opts => opts.MapFrom(src => src.BlindVotes.Sum(x => x.Score)))
                .ForMember(dest => dest.BlindItem, opts => opts.MapFrom(src => src))
                .ForMember(dest => dest.Notes, opts => opts.MapFrom(src => src.BlindVotes.Select(x => x.Note ?? $"{x.Note} - ${x.CreatedByUser.UserName}").ToList()));
        }
    }
}
