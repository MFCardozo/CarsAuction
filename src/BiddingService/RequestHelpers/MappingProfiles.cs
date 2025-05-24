using BiddingService.DTOs;
using Contracts;
using AutoMapper;

namespace BiddingService.RequestHelpers;

public class MappingProfiles : Profile
{
public MappingProfiles()
    {
        CreateMap<Bid, BidDto>();
        CreateMap<Bid, BidPlaced>();
    }
}
