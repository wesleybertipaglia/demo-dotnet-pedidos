using Pedidos.Application.DTOs;

namespace Pedidos.Application.Interfaces
{
    public interface IItemPedidoService
    {
        Task<PedidoReadDTO> AddItemPedido(Guid pedidoId, ItemPedidoAddDTO itemPedidoAddDTO);
        Task<PedidoReadDTO> RemoveItemPedido(Guid pedidoId, ItemPedidoRemoveDTO itemPedidoRemoveDTO);
        Task<PedidoReadDTO> UpdateItemPedido(Guid pedidoId, ItemPedidoUpdateDTO itemPedidoUpdateDTO);
    }
}