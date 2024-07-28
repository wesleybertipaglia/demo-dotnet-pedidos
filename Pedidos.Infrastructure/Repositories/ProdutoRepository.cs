using Pedidos.Domain.Interfaces;
using Pedidos.Domain.Entities;
using Pedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Pedidos.Infrastructure.Exceptions;

namespace Pedidos.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Produto> Produtos, int TotalItems)> GetAllProdutos(int pageNumber, int pageSize, string? titulo, float? precoMin, float? precoMax)
        {
            var query = _context.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(p => p.Titulo.Contains(titulo));

            if (precoMin.HasValue)
                query = query.Where(p => p.Preco >= precoMin.Value);

            if (precoMax.HasValue)
                query = query.Where(p => p.Preco <= precoMax.Value);

            var totalItems = await query.CountAsync();
            var produtos = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (produtos, totalItems);
        }

        public async Task<Produto> GetProdutoById(Guid id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) throw new NotFoundException("Produto não encontrado");
            return produto;
        }

        public async Task<Produto> CreateProduto(Produto produto)
        {
            var produtoCreate = _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produtoCreate.Entity;
        }

        public async Task<Produto> UpdateProduto(Produto produto)
        {
            var produtoUpdate = _context.Produtos.Update(produto);

            await _context.SaveChangesAsync();
            return produtoUpdate.Entity;
        }

        public async Task DeleteProduto(Guid id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) throw new NotFoundException("Produto não encontrado");

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }
    }
}
