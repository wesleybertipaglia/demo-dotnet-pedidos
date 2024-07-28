using Pedidos.Application.DTOs;

namespace Pedidos.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PaginacaoResultDTO<PedidoReadDTO>> GetAllPedidos(int pageNumber, int pageSize);
        Task<PedidoReadDTO> GetPedidoById(Guid id);
        Task<PedidoReadDTO> CreatePedido();
        Task<PedidoReadDTO> ClosePedido(Guid id);
        Task DeletePedido(Guid id);
    }
}