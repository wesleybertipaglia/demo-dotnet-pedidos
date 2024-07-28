namespace Pedidos.Application.DTOs
{
    public record PaginacaoResultDTO<T>(IEnumerable<T> Items, int TotalItems, int PageNumber, int PageSize, int TotalPages);
}
