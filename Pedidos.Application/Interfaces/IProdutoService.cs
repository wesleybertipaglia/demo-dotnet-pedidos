using Pedidos.Application.DTOs;

namespace Pedidos.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<PaginacaoResultDTO<ProdutoReadDTO>> GetAllProdutos(int pageNumber, int pageSize);
        Task<ProdutoReadDTO> GetProdutoById(Guid id);
        Task<ProdutoReadDTO> CreateProduto(ProdutoCreateDTO produtoCreateDTO);
        Task<ProdutoReadDTO> UpdateProduto(Guid id, ProdutoUpdateDTO produtoUpdateDTO);
        Task DeleteProduto(Guid id);
    }
}