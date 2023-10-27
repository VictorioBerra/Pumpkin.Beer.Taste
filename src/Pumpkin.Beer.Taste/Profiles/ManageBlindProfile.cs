namespace Pumpkin.Beer.Taste.Profiles;

using AutoMapper;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;

public class ManageBlindProfile : Profile
{
    public ManageBlindProfile()
    {
        this.CreateMap<Blind, IndexViewModel>()
            .ForMember(dest => dest.HasVotes, opts => opts.MapFrom(src => src.BlindItems.Any(x => x.BlindVotes.Any())));

        this.CreateMap<Blind, CloseViewModel>();

        this.CreateMap<Blind, DeleteViewModel>();

        this.CreateMap<CreateViewModel, Blind>().ReverseMap();
        this.CreateMap<CreateItemViewModel, BlindItem>();

        this.CreateMap<EditViewModel, Blind>().ReverseMap();
        this.CreateMap<EditItemViewModel, BlindItem>().ReverseMap();
    }
}
