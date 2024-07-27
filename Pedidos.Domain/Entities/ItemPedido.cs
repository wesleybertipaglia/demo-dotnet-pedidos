namespace Pedidos.Domain.Entities
{
    public class ItemPedido
    {
        public Guid Id { get; set; }
        public int Quantidade { get; set; }
        public float Preco { get; set; }
        public float Total => Quantidade * Preco;
        public Pedido Pedido { get; set; } = null!;
        public Produto Produto { get; set; } = null!;

        public ItemPedido()
        {
            Id = Guid.NewGuid();
        }

        public ItemPedido(int quantidade, float preco, Produto produto, Pedido pedido)
        {
            Id = Guid.NewGuid();
            Quantidade = quantidade;
            Preco = preco;
            Pedido = pedido;
            Produto = produto;
        }
    }
}
