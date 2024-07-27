namespace Pedidos.Application.DTOs
{
    public record PedidoReadDTO(Guid Id, string Status, float Total, List<ItemPedidoReadDTO> ItensPedidos);
}