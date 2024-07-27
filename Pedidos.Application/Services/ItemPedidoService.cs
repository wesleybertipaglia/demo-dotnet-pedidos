using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;
using Pedidos.Domain.Interfaces;
using AutoMapper;

namespace Pedidos.Application.Services
{
    public class ItemPedidoService : IItemPedidoService
    {
        private readonly IItemPedidoRepository _itemPedidoRepository;
        private readonly IMapper _mapper;

        public ItemPedidoService(IItemPedidoRepository itemPedidoRepository, IMapper mapper)
        {
            _itemPedidoRepository = itemPedidoRepository;
            _mapper = mapper;
        }

        public async Task<PedidoReadDTO> AddItemPedido(Guid pedidoId, ItemPedidoAddDTO itemPedidoAddDTO)
        {
            var pedido = await _itemPedidoRepository.AddItemPedido(
                pedidoId,
                itemPedidoAddDTO.ProdutoId,
                itemPedidoAddDTO.Quantidade);

            return _mapper.Map<PedidoReadDTO>(pedido);
        }

        public async Task<PedidoReadDTO> RemoveItemPedido(Guid pedidoId, ItemPedidoRemoveDTO itemPedidoRemoveDTO)
        {
            var pedido = await _itemPedidoRepository.RemoveItemPedido(
                pedidoId,
                itemPedidoRemoveDTO.ProdutoId);

            return _mapper.Map<PedidoReadDTO>(pedido);
        }

        public async Task<PedidoReadDTO> UpdateItemPedido(Guid pedidoId, ItemPedidoUpdateDTO itemPedidoUpdateDTO)
        {
            var pedido = await _itemPedidoRepository.UpdateItemPedido(
                pedidoId,
                itemPedidoUpdateDTO.ProdutoId,
                itemPedidoUpdateDTO.Quantidade);

            return _mapper.Map<PedidoReadDTO>(pedido);
        }
    }
}
