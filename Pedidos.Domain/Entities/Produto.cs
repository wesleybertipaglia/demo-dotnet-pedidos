namespace Pedidos.Domain.Entities
{
    public class Produto
    {
        public Guid Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public float Preco { get; set; }

        public Produto()
        {
        }

        public Produto(string titulo, string descricao, float preco)
        {
            Id = Guid.NewGuid();
            Titulo = titulo;
            Descricao = descricao;
            Preco = preco;
        }
    }
}
