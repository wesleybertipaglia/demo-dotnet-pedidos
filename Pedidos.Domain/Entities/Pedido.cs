using Pedidos.Domain.Enums;

namespace Pedidos.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public StatusPedido Status { get; set; }
        public List<ItemPedido> ItensPedidos { get; set; }
        public float Total => ItensPedidos.Sum(i => i.Total);

        public Pedido()
        {
            Id = Guid.NewGuid();
            Status = StatusPedido.Aberto;
            ItensPedidos = new List<ItemPedido>();
        }

        public Pedido(StatusPedido status, List<ItemPedido> itensPedidos)
        {
            Id = Guid.NewGuid();
            Status = status;
            ItensPedidos = itensPedidos ?? new List<ItemPedido>();
        }
    }
}
