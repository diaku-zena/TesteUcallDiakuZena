using AutoMapper;
using Revisao.Application.DTOs;
using Revisao.Domain.Entities;

namespace Revisao.Application.AutoMapper;

public class DomainToDTOMapping : Profile
{
    public DomainToDTOMapping()
    {
        CreateMap<CreateOrderRequest, Order>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<Order, CreateOrderRequest>().ReverseMap();
        CreateMap<OrderItem, OrderItemDTO>().ReverseMap();

        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
