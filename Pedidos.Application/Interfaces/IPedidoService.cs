using Pedidos.Application.DTOs;

namespace Pedidos.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PaginacaoResultDTO<PedidoReadDTO>> GetAllPedidos(int pageNumber, int pageSize, string? status, float? totalMin, float? totalMax);
        Task<PedidoReadDTO> GetPedidoById(Guid id);
        Task<PedidoReadDTO> CreatePedido();
        Task<PedidoReadDTO> ClosePedido(Guid id);
        Task DeletePedido(Guid id);
    }
}