using Pedidos.Domain.Entities;

namespace Pedidos.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<(IEnumerable<Produto> Produtos, int TotalItems)> GetAllProdutos(int pageNumber, int pageSize, string? titulo, float? precoMin, float? precoMax);
        Task<Produto> GetProdutoById(Guid id);
        Task<Produto> CreateProduto(Produto produto);
        Task<Produto> UpdateProduto(Produto produto);
        Task DeleteProduto(Guid id);
    }
}