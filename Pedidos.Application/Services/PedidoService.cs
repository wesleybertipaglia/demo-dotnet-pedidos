using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;
using Pedidos.Domain.Interfaces;
using AutoMapper;

namespace Pedidos.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public PedidoService(IPedidoRepository pedidoRepository, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }

        public async Task<PaginacaoResultDTO<PedidoReadDTO>> GetAllPedidos(int pageNumber, int pageSize, string? status, float? totalMin, float? totalMax)
        {
            var (pedidos, totalItems) = await _pedidoRepository.GetAllPedidos(pageNumber, pageSize, status, totalMin, totalMax);
            var pedidosDto = _mapper.Map<IEnumerable<PedidoReadDTO>>(pedidos);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new PaginacaoResultDTO<PedidoReadDTO>(pedidosDto, totalItems, pageNumber, pageSize, totalPages);
        }

        public async Task<PedidoReadDTO> GetPedidoById(Guid id)
        {
            var pedido = await _pedidoRepository.GetPedidoById(id);
            return _mapper.Map<PedidoReadDTO>(pedido);
        }

        public async Task<PedidoReadDTO> CreatePedido()
        {
            var pedido = await _pedidoRepository.CreatePedido();
            return _mapper.Map<PedidoReadDTO>(pedido);
        }

        public async Task<PedidoReadDTO> ClosePedido(Guid id)
        {
            var pedido = await _pedidoRepository.ClosePedido(id);
            return _mapper.Map<PedidoReadDTO>(pedido);
        }

        public async Task DeletePedido(Guid id)
        {
            await _pedidoRepository.DeletePedido(id);
        }
    }
}
