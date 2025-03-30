using System;
using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.requestHelpers;

public class MappingProfiles : Profile
{

public MappingProfiles()
{
    CreateMap<AuctionCreated, Item>();
    CreateMap<AuctionUpdated, Item>();
    CreateMap<AuctionDeleted, Item>();
}
}
