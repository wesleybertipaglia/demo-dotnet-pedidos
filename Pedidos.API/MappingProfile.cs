using AutoMapper;
using Pedidos.Application.DTOs;
using Pedidos.Domain.Entities;

namespace Pedidos.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produto, ProdutoReadDTO>();
            CreateMap<ProdutoCreateDTO, Produto>();
            CreateMap<ProdutoUpdateDTO, Produto>();
            CreateMap<Pedido, PedidoReadDTO>();
            CreateMap<ItemPedido, ItemPedidoReadDTO>();
        }
    }
}
