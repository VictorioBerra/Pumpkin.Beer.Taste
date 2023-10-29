namespace Pumpkin.Beer.Taste.Profiles;

using AutoMapper;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.ViewModels.Home;

public class HomeProfile : Profile
{
    public HomeProfile() =>
        this.CreateMap<Blind, IndexViewModel>()
            .ForMember(dest => dest.NumMembers, opts => opts.MapFrom(src => src.UserInvites.Count()));
}
