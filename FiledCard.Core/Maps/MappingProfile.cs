using AutoMapper;
using FiledCard.Core.DTOs;
using FiledCard.Core.Entities;

namespace FiledCard.Core.Maps
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentDto, Payment>();
        }
    }
}
