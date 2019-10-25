﻿using AutoMapper;
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

        }
    }
}
