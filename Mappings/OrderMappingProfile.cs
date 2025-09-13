using AutoMapper;
using OrderManagementSystem.DTOs.Order;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<CreateOrderDTO, OrderModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Costumer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src =>
                src.OrderDate ?? DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.Status ?? OrderStatusEnum.Pendente));

        CreateMap<UpdateOrderDTO, OrderModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Costumer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<OrderModel, OrderResponseDTO>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Costumer));

        CreateMap<OrderModel, OrderListDTO>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Costumer));
    }
}