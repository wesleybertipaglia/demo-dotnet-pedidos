using Microsoft.AspNetCore.Mvc;
using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;

namespace Pedidos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoReadDTO>>> GetAllPedidos()
        {
            var pedidos = await _pedidoService.GetAllPedidos();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoReadDTO>> GetPedidoById(Guid id)
        {
            var pedido = await _pedidoService.GetPedidoById(id);
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoReadDTO>> CreatePedido()
        {
            var pedido = await _pedidoService.CreatePedido();
            return CreatedAtAction(nameof(GetPedidoById), new { id = pedido.Id }, pedido);
        }

        [HttpPost("{id}/close")]
        public async Task<ActionResult<PedidoReadDTO>> ClosePedido(Guid id)
        {
            var pedido = await _pedidoService.ClosePedido(id);
            return Ok(pedido);
        }

        [HttpPost("{id}/itens")]
        public async Task<ActionResult<PedidoReadDTO>> AddItemPedido(Guid id, Guid produtoId, int quantidade)
        {
            var pedido = await _pedidoService.AddItemPedido(id, produtoId, quantidade);
            return Ok(pedido);
        }

        [HttpDelete("{id}/itens/{produtoId}")]
        public async Task<ActionResult<PedidoReadDTO>> RemoveItemPedido(Guid id, Guid produtoId)
        {
            var pedido = await _pedidoService.RemoveItemPedido(id, produtoId);
            return Ok(pedido);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(Guid id)
        {
            await _pedidoService.DeletePedido(id);
            return NoContent();
        }
    }
}
