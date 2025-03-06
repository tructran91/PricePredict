using AutoMapper;
using PricePrediction.Application.Responses;
using PricePrediction.Core.Entities;

namespace PricePrediction.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TradeSignalResponse, TradeSignal>();
        }
    }
}
