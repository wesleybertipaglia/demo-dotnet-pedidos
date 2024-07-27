using Pedidos.Application.DTOs;

namespace Pedidos.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoReadDTO>> GetAllPedidos();
        Task<PedidoReadDTO> GetPedidoById(Guid id);
        Task<PedidoReadDTO> CreatePedido();
        Task<PedidoReadDTO> ClosePedido(Guid id);
        Task DeletePedido(Guid id);
        Task<PedidoReadDTO> AddItemPedido(Guid id, Guid produtoId, int quantidade);
        Task<PedidoReadDTO> RemoveItemPedido(Guid id, Guid produtoId);
    }
}