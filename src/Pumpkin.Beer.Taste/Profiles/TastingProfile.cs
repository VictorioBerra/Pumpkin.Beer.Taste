namespace Pumpkin.Beer.Taste.Profiles;

using AutoMapper;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Eblindtensions;
using Pumpkin.Beer.Taste.ViewModels.Tastings;

public class TastingProfile : Profile
{
    public TastingProfile() =>
        this.CreateMap<Blind, IndexViewModel>()
            .ForMember(dest => dest.NumMembers, opts => opts.MapFrom(src => src.UserInvites.Count()));
}
