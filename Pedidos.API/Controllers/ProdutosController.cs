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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoReadDTO>>> GetAllProdutos()
        {
            var produtos = await _produtoService.GetAllProdutos();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoReadDTO>> GetProdutoById(Guid id)
        {
            var produto = await _produtoService.GetProdutoById(id);
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoReadDTO>> CreateProduto([FromBody] ProdutoCreateDTO produtoCreateDTO)
        {
            var produto = await _produtoService.CreateProduto(produtoCreateDTO);
            return CreatedAtAction(nameof(GetProdutoById), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoReadDTO>> UpdateProduto(Guid id, [FromBody] ProdutoUpdateDTO produtoUpdateDTO)
        {
            var produto = await _produtoService.UpdateProduto(id, produtoUpdateDTO);
            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(Guid id)
        {
            await _produtoService.DeleteProduto(id);
            return NoContent();
        }
    }
}
