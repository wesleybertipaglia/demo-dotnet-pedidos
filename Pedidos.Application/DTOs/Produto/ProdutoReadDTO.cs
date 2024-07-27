namespace Pedidos.Application.DTOs
{
    public record ProdutoReadDTO(Guid Id, string Titulo, string Descricao, float Preco);
}