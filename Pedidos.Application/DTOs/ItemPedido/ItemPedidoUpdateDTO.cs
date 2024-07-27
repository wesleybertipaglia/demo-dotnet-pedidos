namespace Pedidos.Application.DTOs
{
    public record ItemPedidoUpdateDTO(Guid ProdutoId, int Quantidade = 1);
}