using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Enums;
using Pedidos.Domain.Interfaces;
using Pedidos.Infrastructure.Data;
using Pedidos.Infrastructure.Exceptions;

namespace Pedidos.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Pedido> Pedidos, int TotalItems)> GetAllPedidos(int pageNumber, int pageSize, string? status, float? totalMin, float? totalMax)
        {
            var query = _context.Pedidos.Include(p => p.ItensPedidos).AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(p => p.Status.ToString().ToLower() == status.ToLower());

            var pedidos = await query.ToListAsync();

            if (totalMin.HasValue)
            {
                pedidos = pedidos.Where(p => p.ItensPedidos.Sum(i => i.Total) >= totalMin.Value).ToList();
            }

            if (totalMax.HasValue)
            {
                pedidos = pedidos.Where(p => p.ItensPedidos.Sum(i => i.Total) <= totalMax.Value).ToList();
            }

            var totalItems = await query.CountAsync();
            pedidos = pedidos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return (pedidos, totalItems);
        }

        public async Task<Pedido> GetPedidoById(Guid id)
        {
            var pedido = await _context.Pedidos.Include(p => p.ItensPedidos)
                .ThenInclude(i => i.Produto).SingleOrDefaultAsync(p => p.Id == id)
                ?? throw new NotFoundException("Pedido não encontrado");

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
                ?? throw new NotFoundException("Pedido não encontrado");

            if (pedido.ItensPedidos.Count == 0)
                throw new BadRequestException("Para fechar um pedido, é necessário adicionar itens");

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
    }
}
