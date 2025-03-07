using AutoMapper;
using PricePrediction.Application.Responses;
using PricePrediction.Core.Entities;

namespace PricePrediction.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TradeSignalResponse, TradeSignal>()
                .ForMember(dest => dest.TradeId, opt => opt.MapFrom(src => Guid.Parse(src.TradeId)));

            CreateMap<TradeSignal, TradeSignalResponse>();
        }
    }
}
