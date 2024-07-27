namespace Pedidos.Application.DTOs
{
    public record ItemPedidoAddDTO(Guid ProdutoId, int Quantidade = 1);
}