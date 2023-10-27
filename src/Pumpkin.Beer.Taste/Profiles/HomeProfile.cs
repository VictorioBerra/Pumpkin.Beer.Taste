namespace Pumpkin.Beer.Taste.Profiles;

using AutoMapper;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.ViewModels.Home;

public class HomeProfile : Profile
{
    public HomeProfile() => this.CreateMap<Blind, IndexViewModel>();
}
