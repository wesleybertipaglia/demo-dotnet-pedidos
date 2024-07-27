using Pedidos.Domain.Entities;

namespace Pedidos.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetAllProdutos();
        Task<Produto> GetProdutoById(Guid id);
        Task<Produto> CreateProduto(Produto produto);
        Task<Produto> UpdateProduto(Produto produto);
        Task DeleteProduto(Guid id);
    }
}