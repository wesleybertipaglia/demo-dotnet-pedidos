using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Enums;
using Pedidos.Domain.Interfaces;
using Pedidos.Infrastructure.Data;

namespace Pedidos.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> GetAllPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.ItensPedidos).ThenInclude(i => i.Produto).ToListAsync();

            return pedidos;
        }

        public async Task<Pedido> GetPedidoById(Guid id)
        {
            var pedido = await _context.Pedidos.Include(p => p.ItensPedidos)
                .ThenInclude(i => i.Produto).SingleOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Pedido não encontrado");

            return pedido;
        }

        public async Task<Pedido> CreatePedido()
        {
            var pedidoCreate = _context.Pedidos.Add(new Pedido());
            await _context.SaveChangesAsync();
            return pedidoCreate.Entity;
        }

        public async Task<Pedido> ClosePedido(Guid id)
        {
            var pedido = await _context.Pedidos.Include(p => p.ItensPedidos)
                .ThenInclude(i => i.Produto).SingleOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Pedido não encontrado");

            if (pedido.ItensPedidos.Count == 0)
                throw new Exception("Para fechar um pedido, é necessário adicionar itens");

            pedido.Status = StatusPedido.Fechado;
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task DeletePedido(Guid id)
        {
            var pedido = await _context.Pedidos.FindAsync(id) ?? throw new Exception("Pedido não encontrado");
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<Pedido> AddItemPedido(Guid id, Guid produtoId, int quantidade)
        {
            var pedido = await _context.Pedidos.Include(p => p.ItensPedidos).SingleOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Pedido não encontrado");

            var produto = await _context.Produtos.FindAsync(produtoId) ?? throw new Exception("Produto não encontrado");

            if (pedido.Status == StatusPedido.Fechado)
                throw new Exception("Não é possível adicionar itens a um pedido fechado");

            if (quantidade <= 0)
                throw new Exception("A quantidade deve ser maior que zero");

            var itemExiste = pedido.ItensPedidos.FirstOrDefault(i => i.Produto == produto);

            if (itemExiste != null)
            {
                itemExiste.Quantidade += quantidade;
                await _context.SaveChangesAsync();
                return pedido;
            }

            var itemPedido = new ItemPedido(quantidade, produto.Preco, produto);
            _context.ItensPedidos.Add(itemPedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> RemoveItemPedido(Guid id, Guid produtoId)
        {
            var pedido = await _context.Pedidos.FindAsync(id) ?? throw new Exception("Pedido não encontrado");
            var produto = await _context.Produtos.FindAsync(produtoId) ?? throw new Exception("Produto não encontrado");
            var itemPedido = pedido.ItensPedidos.FirstOrDefault(i => i.Id == produtoId) ?? throw new Exception("Item não encontrado");

            if (pedido.Status == StatusPedido.Fechado)
                throw new Exception("Não é possível remover itens de um pedido fechado");

            pedido.ItensPedidos.Remove(itemPedido);
            await _context.SaveChangesAsync();
            return pedido;
        }
    }
}
