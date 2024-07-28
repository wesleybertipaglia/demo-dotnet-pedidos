using System.Text.Json;
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
        private readonly IItemPedidoService _itemPedidoService;

        public PedidosController(IPedidoService pedidoService, IItemPedidoService itemPedidoService)
        {
            _pedidoService = pedidoService;
            _itemPedidoService = itemPedidoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPedidos(int pageNumber = 1, int pageSize = 10)
        {
            var paginacaoResult = await _pedidoService.GetAllPedidos(pageNumber, pageSize);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginacaoResult);

            return Ok(paginacaoResult.Items);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(Guid id)
        {
            await _pedidoService.DeletePedido(id);
            return NoContent();
        }

        [HttpPost("{id}/itens")]
        public async Task<ActionResult<PedidoReadDTO>> AddItemPedido(Guid id, ItemPedidoAddDTO itemPedidoAddDTO)
        {
            var pedido = await _itemPedidoService.AddItemPedido(id, itemPedidoAddDTO);
            return Ok(pedido);
        }

        [HttpPut("{id}/itens")]
        public async Task<ActionResult<PedidoReadDTO>> UpdateItemPedido(Guid id, ItemPedidoUpdateDTO itemPedidoUpdateDTO)
        {
            var pedido = await _itemPedidoService.UpdateItemPedido(id, itemPedidoUpdateDTO);
            return Ok(pedido);
        }

        [HttpDelete("{id}/itens")]
        public async Task<ActionResult<PedidoReadDTO>> RemoveItemPedido(Guid id, ItemPedidoRemoveDTO itemPedidoRemoveDTO)
        {
            var pedido = await _itemPedidoService.RemoveItemPedido(id, itemPedidoRemoveDTO);
            return Ok(pedido);
        }
    }
}
