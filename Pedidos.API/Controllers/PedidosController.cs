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

        /// <summary>
        /// Retorna todos os pedidos com base nos parâmetros fornecidos.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="totalMin"></param>
        /// <param name="totalMax"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Lista paginada de pedidos.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPedidos(string? status, float? totalMin, float? totalMax, int pageNumber = 1, int pageSize = 10)
        {
            var paginacaoResult = await _pedidoService.GetAllPedidos(pageNumber, pageSize, status, totalMin, totalMax);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginacaoResult);

            return Ok(paginacaoResult.Items);
        }

        /// <summary>
        /// Retorna um pedido com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Pedido.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoReadDTO>> GetPedidoById(Guid id)
        {
            var pedido = await _pedidoService.GetPedidoById(id);
            return Ok(pedido);
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <returns>Pedido.</returns>
        [HttpPost]
        public async Task<ActionResult<PedidoReadDTO>> CreatePedido()
        {
            var pedido = await _pedidoService.CreatePedido();
            return CreatedAtAction(nameof(GetPedidoById), new { id = pedido.Id }, pedido);
        }

        /// <summary>
        /// Fecha um pedido com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Pedido.</returns>
        [HttpPost("{id}/close")]
        public async Task<ActionResult<PedidoReadDTO>> ClosePedido(Guid id)
        {
            var pedido = await _pedidoService.ClosePedido(id);
            return Ok(pedido);
        }

        /// <summary>
        /// Deleta um pedido com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Pedido.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(Guid id)
        {
            await _pedidoService.DeletePedido(id);
            return NoContent();
        }

        /// <summary>
        /// Adiciona um item ao pedido com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemPedidoAddDTO"></param>
        /// <returns>Pedido.</returns>
        [HttpPost("{id}/itens")]
        public async Task<ActionResult<PedidoReadDTO>> AddItemPedido(Guid id, ItemPedidoAddDTO itemPedidoAddDTO)
        {
            var pedido = await _itemPedidoService.AddItemPedido(id, itemPedidoAddDTO);
            return Ok(pedido);
        }

        /// <summary>
        /// Atualiza um item do pedido com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemPedidoUpdateDTO"></param>
        /// <returns>Pedido.</returns>
        [HttpPut("{id}/itens")]
        public async Task<ActionResult<PedidoReadDTO>> UpdateItemPedido(Guid id, ItemPedidoUpdateDTO itemPedidoUpdateDTO)
        {
            var pedido = await _itemPedidoService.UpdateItemPedido(id, itemPedidoUpdateDTO);
            return Ok(pedido);
        }

        /// <summary>
        /// Remove um item do pedido com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemPedidoRemoveDTO"></param>
        /// <returns>Pedido.</returns>
        [HttpDelete("{id}/itens")]
        public async Task<ActionResult<PedidoReadDTO>> RemoveItemPedido(Guid id, ItemPedidoRemoveDTO itemPedidoRemoveDTO)
        {
            var pedido = await _itemPedidoService.RemoveItemPedido(id, itemPedidoRemoveDTO);
            return Ok(pedido);
        }
    }
}
