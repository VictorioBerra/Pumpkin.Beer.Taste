namespace Pumpkin.Beer.Taste.Profiles;

using AutoMapper;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;

public class ManageBlindProfile : Profile
{
    public ManageBlindProfile()
    {
        this.CreateMap<Blind, IndexViewModel>()
            .ForMember(dest => dest.NumMembers, opts => opts.MapFrom(src => src.UserInvites.Count()))
            .ForMember(dest => dest.NumItems, opts => opts.MapFrom(src => src.BlindItems.Count()))
            .ForMember(dest => dest.NumVotes, opts => opts.MapFrom(src => src.BlindItems.SelectMany(x => x.BlindVotes).Count()));

        this.CreateMap<Blind, CloseViewModel>();

        this.CreateMap<Blind, DeleteViewModel>();

        this.CreateMap<CreateViewModel, Blind>().ReverseMap();
        this.CreateMap<CreateItemViewModel, BlindItem>();

        this.CreateMap<EditViewModel, Blind>().ReverseMap();
        this.CreateMap<EditItemViewModel, BlindItem>().ReverseMap();
    }
}
