using Pedidos.Application.Interfaces;
using Pedidos.Application.DTOs;
using Pedidos.Domain.Interfaces;
using Pedidos.Domain.Entities;
using AutoMapper;

namespace Pedidos.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ProdutoService(IProdutoRepository produtoRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public async Task<PaginacaoResultDTO<ProdutoReadDTO>> GetAllProdutos(int pageNumber, int pageSize)
        {
            var (produtos, totalItems) = await _produtoRepository.GetAllProdutos(pageNumber, pageSize);
            var produtosDto = _mapper.Map<IEnumerable<ProdutoReadDTO>>(produtos);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new PaginacaoResultDTO<ProdutoReadDTO>(produtosDto, totalItems, pageNumber, pageSize, totalPages);
        }

        public async Task<ProdutoReadDTO> GetProdutoById(Guid id)
        {
            var produto = await _produtoRepository.GetProdutoById(id);
            return _mapper.Map<ProdutoReadDTO>(produto);
        }

        public async Task<ProdutoReadDTO> CreateProduto(ProdutoCreateDTO produtoCreateDTO)
        {
            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Titulo = produtoCreateDTO.Titulo,
                Descricao = produtoCreateDTO.Descricao,
                Preco = produtoCreateDTO.Preco
            };

            produto = await _produtoRepository.CreateProduto(produto);
            return _mapper.Map<ProdutoReadDTO>(produto);
        }

        public async Task<ProdutoReadDTO> UpdateProduto(Guid id, ProdutoUpdateDTO produtoUpdateDTO)
        {
            var produto = await _produtoRepository.GetProdutoById(id);
            if (produto == null) throw new Exception("Produto não encontrado");

            if (produtoUpdateDTO.Titulo != null) produto.Titulo = produtoUpdateDTO.Titulo;
            if (produtoUpdateDTO.Descricao != null) produto.Descricao = produtoUpdateDTO.Descricao;
            if (produtoUpdateDTO.Preco != null) produto.Preco = produtoUpdateDTO.Preco.Value;

            produto = await _produtoRepository.UpdateProduto(produto);
            return _mapper.Map<ProdutoReadDTO>(produto);
        }

        public async Task DeleteProduto(Guid id)
        {
            await _produtoRepository.DeleteProduto(id);
        }
    }
}
