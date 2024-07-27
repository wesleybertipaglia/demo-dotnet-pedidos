using Pedidos.Domain.Entities;

namespace Pedidos.Domain.Interfaces
{
    public interface IItemPedidoRepository
    {
        Task<Pedido> AddItemPedido(Guid pedidoId, Guid produtoId, int quantidade);
        Task<Pedido> RemoveItemPedido(Guid pedidoId, Guid produtoId);
        Task<Pedido> UpdateItemPedido(Guid pedidoId, Guid produtoId, int quantidade);
    }
}