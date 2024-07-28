using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;

namespace Pedidos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
        }

        /// <summary>
        /// Retorna todos os produtos com base nos parâmetros fornecidos.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="precoMin"></param>
        /// <param name="precoMax"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Lista paginada de produtos.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProdutos(string? titulo, float? precoMin, float? precoMax, int pageNumber = 1, int pageSize = 10)
        {
            var paginacaoResult = await _produtoService.GetAllProdutos(pageNumber, pageSize, titulo, precoMin, precoMax);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginacaoResult);

            return Ok(paginacaoResult.Items);
        }

        /// <summary>
        /// Retorna um produto com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Produto.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoReadDTO>> GetProdutoById(Guid id)
        {
            var produto = await _produtoService.GetProdutoById(id);
            return Ok(produto);
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="produtoCreateDTO"></param>
        /// <returns>Produto.</returns>
        [HttpPost]
        public async Task<ActionResult<ProdutoReadDTO>> CreateProduto([FromBody] ProdutoCreateDTO produtoCreateDTO)
        {
            var produto = await _produtoService.CreateProduto(produtoCreateDTO);
            return CreatedAtAction(nameof(GetProdutoById), new { id = produto.Id }, produto);
        }

        /// <summary>
        /// Atualiza um produto com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="produtoUpdateDTO"></param>
        /// <returns>Produto.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoReadDTO>> UpdateProduto(Guid id, [FromBody] ProdutoUpdateDTO produtoUpdateDTO)
        {
            var produto = await _produtoService.UpdateProduto(id, produtoUpdateDTO);
            return Ok(produto);
        }

        /// <summary>
        /// Deleta um produto com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(Guid id)
        {
            await _produtoService.DeleteProduto(id);
            return NoContent();
        }
    }
}
