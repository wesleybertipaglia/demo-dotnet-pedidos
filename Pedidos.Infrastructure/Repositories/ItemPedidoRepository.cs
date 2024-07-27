using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Enums;
using Pedidos.Domain.Interfaces;
using Pedidos.Infrastructure.Data;
using Pedidos.Infrastructure.Exceptions;

namespace Pedidos.Infrastructure.Repositories
{
    public class ItemPedidoRepository : IItemPedidoRepository
    {
        private readonly AppDbContext _context;

        public ItemPedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido> AddItemPedido(Guid pedidoId, Guid produtoId, int quantidade)
        {
            var pedido = await _context.Pedidos.Include(p => p.ItensPedidos).SingleOrDefaultAsync(p => p.Id == pedidoId)
                ?? throw new NotFoundException("Pedido não encontrado");

            var produto = await _context.Produtos.FindAsync(produtoId) ?? throw new Exception("Produto não encontrado");

            if (pedido.Status == StatusPedido.Fechado)
                throw new BadRequestException("Não é possível adicionar itens a um pedido fechado");

            if (quantidade <= 0)
                throw new BadRequestException("A quantidade deve ser maior que zero");

            var itemExiste = pedido.ItensPedidos.FirstOrDefault(i => i.Produto == produto);

            if (itemExiste != null)
            {
                itemExiste.Quantidade += quantidade;
                await _context.SaveChangesAsync();
                return pedido;
            }

            var itemPedido = new ItemPedido(quantidade, produto.Preco, produto, pedido);
            _context.ItensPedidos.Add(itemPedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> RemoveItemPedido(Guid pedidoId, Guid produtoId)
        {
            var pedido = await _context.Pedidos.Include(p => p.ItensPedidos).SingleOrDefaultAsync(p => p.Id == pedidoId)
                ?? throw new NotFoundException("Pedido não encontrado");

            var produto = await _context.Produtos.FindAsync(produtoId)
                ?? throw new NotFoundException("Produto não encontrado");

            var itemPedido = pedido.ItensPedidos.FirstOrDefault(i => i.Produto.Id == produtoId)
                ?? throw new NotFoundException("Item não encontrado");

            if (pedido.Status == StatusPedido.Fechado)
                throw new BadRequestException("Não é possível remover itens de um pedido fechado");

            pedido.ItensPedidos.Remove(itemPedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> UpdateItemPedido(Guid pedidoId, Guid produtoId, int quantidade)
        {
            var pedido = await _context.Pedidos.Include(p => p.ItensPedidos).SingleOrDefaultAsync(p => p.Id == pedidoId)
                ?? throw new NotFoundException("Pedido não encontrado");

            var produto = await _context.Produtos.FindAsync(produtoId)
                ?? throw new NotFoundException("Produto não encontrado");

            var itemPedido = pedido.ItensPedidos.FirstOrDefault(i => i.Produto.Id == produtoId)
                ?? throw new NotFoundException("Item não encontrado");

            if (pedido.Status == StatusPedido.Fechado)
                throw new BadRequestException("Não é possível atualizar itens de um pedido fechado");

            if (quantidade <= 0)
                throw new BadRequestException("A quantidade deve ser maior que zero");

            itemPedido.Quantidade = quantidade;
            await _context.SaveChangesAsync();
            return pedido;
        }
    }
}
