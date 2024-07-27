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

        public async Task<IEnumerable<PedidoReadDTO>> GetAllPedidos()
        {
            var pedidos = await _pedidoRepository.GetAllPedidos();
            return _mapper.Map<IEnumerable<PedidoReadDTO>>(pedidos);
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
