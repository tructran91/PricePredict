using AutoMapper;
using DataImport.Application.Responses;
using DataImport.Core.Entities;

namespace DataImport.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Candlestick, CandlestickResponse>();
        }
    }
}
