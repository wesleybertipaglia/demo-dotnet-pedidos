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

        public async Task<(IEnumerable<Produto> Produtos, int TotalItems)> GetAllProdutos(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Produtos.CountAsync();
            var produtos = await _context.Produtos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
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
