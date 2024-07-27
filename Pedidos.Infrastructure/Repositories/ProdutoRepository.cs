using Pedidos.Domain.Interfaces;
using Pedidos.Domain.Entities;
using Pedidos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Pedidos.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetAllProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto> GetProdutoById(Guid id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) throw new Exception("Produto não encontrado");
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
            if (produto == null) throw new Exception("Produto não encontrado");

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }
    }
}
