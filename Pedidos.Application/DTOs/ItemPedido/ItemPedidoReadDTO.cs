namespace Pedidos.Application.DTOs
{
    public record ItemPedidoReadDTO(Guid PedidoId, Guid ProdutoId, string ProdutoTitulo, int Quantidade, float Preco, float Total);
}