namespace Pumpkin.Beer.Taste.Profiles;

using AutoMapper;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.ViewModels.Vote;

public class VoteProfile : Profile
{
    public VoteProfile()
    {
        this.CreateMap<BlindItem, IndexBlindItemViewModel>()
            .ForMember(dest => dest.Letter, opts => opts.Ignore());

        this.CreateMap<BlindVote, IndexBlindItemViewModel>();

        this.CreateMap<Blind, IndexBlindViewModel>();

        this.CreateMap<IndexViewModel, BlindVote>();
        this.CreateMap<BlindVote, IndexViewModel>();
    }
}
