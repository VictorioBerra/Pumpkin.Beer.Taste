namespace Pumpkin.Beer.Taste.Profiles;

using AutoMapper;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.ViewModels.Scores;

public class ScoresProfile : Profile
{
    public ScoresProfile()
    {
        this.CreateMap<Blind, IndexBlindViewModel>();

        this.CreateMap<BlindItem, IndexBlindItemViewModel>();

        this.CreateMap<BlindVote, IndexVoteViewModel>();

        this.CreateMap<BlindItem, IndexViewModel>()
            .ForMember(dest => dest.AmountOfVotes, opts => opts.MapFrom(src => src.BlindVotes.Count()))
            .ForMember(dest => dest.TotalScore, opts => opts.MapFrom(src => src.BlindVotes.Sum(x => x.Score)))
            .ForMember(dest => dest.AverageScore, opts => opts.MapFrom(src => src.BlindVotes.Average(x => x.Score)))
            .ForMember(dest => dest.Votes, opts => opts.MapFrom(src => src.BlindVotes))
            .ForMember(dest => dest.BlindItem, opts => opts.MapFrom(src => src));
    }
}
