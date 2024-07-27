using Pedidos.Application.DTOs;

namespace Pedidos.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoReadDTO>> GetAllProdutos();
        Task<ProdutoReadDTO> GetProdutoById(Guid id);
        Task<ProdutoReadDTO> CreateProduto(ProdutoCreateDTO produtoCreateDTO);
        Task<ProdutoReadDTO> UpdateProduto(Guid id, ProdutoUpdateDTO produtoUpdateDTO);
        Task DeleteProduto(Guid id);
    }
}