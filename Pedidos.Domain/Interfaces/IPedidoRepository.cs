using Pedidos.Domain.Entities;

namespace Pedidos.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<(IEnumerable<Pedido> Pedidos, int TotalItems)> GetAllPedidos(int pageNumber, int pageSize);
        Task<Pedido> GetPedidoById(Guid id);
        Task<Pedido> CreatePedido();
        Task<Pedido> ClosePedido(Guid id);
        Task DeletePedido(Guid id);
    }
}