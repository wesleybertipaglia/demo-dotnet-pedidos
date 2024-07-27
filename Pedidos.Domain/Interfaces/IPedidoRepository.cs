using Pedidos.Domain.Entities;

namespace Pedidos.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> GetAllPedidos();
        Task<Pedido> GetPedidoById(Guid id);
        Task<Pedido> CreatePedido();
        Task<Pedido> ClosePedido(Guid id);
        Task DeletePedido(Guid id);
        Task<Pedido> AddItemPedido(Guid id, Guid produtoId, int quantidade);
        Task<Pedido> RemoveItemPedido(Guid id, Guid produtoId);
    }
}