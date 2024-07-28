using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Enums;
using Pedidos.Infrastructure.Data;
using Pedidos.Infrastructure.Repositories;

namespace Pedidos.Tests
{
    public class PedidoRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        [Fact]
        public async Task GetAllPedidos_ShouldReturnAllOrders()
        {
            var context = GetInMemoryDbContext();
            var repository = new PedidoRepository(context);

            var pedido1 = new Pedido { Id = Guid.NewGuid(), Status = StatusPedido.Aberto };
            var pedido2 = new Pedido { Id = Guid.NewGuid(), Status = StatusPedido.Fechado };

            context.Pedidos.Add(pedido1);
            context.Pedidos.Add(pedido2);
            await context.SaveChangesAsync();

            var result = await repository.GetAllPedidos(1, 10, null, null, null);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(2, result.Pedidos.Count());
        }

        [Fact]
        public async Task GetPedidoById_ShouldReturnOrder()
        {
            var context = GetInMemoryDbContext();
            var repository = new PedidoRepository(context);
            var pedidoId = Guid.NewGuid();

            context.Pedidos.Add(new Pedido { Id = pedidoId, Status = StatusPedido.Aberto });
            await context.SaveChangesAsync();

            var result = await repository.GetPedidoById(pedidoId);
            Assert.NotNull(result);
            Assert.Equal(pedidoId, result.Id);
        }

        [Fact]
        public async Task CreatePedido_ShouldAddOrder()
        {
            var context = GetInMemoryDbContext();
            var repository = new PedidoRepository(context);

            var result = await repository.CreatePedido();
            var pedido = await context.Pedidos.FindAsync(result.Id);
            Assert.NotNull(pedido);
            Assert.Equal(result.Id, pedido.Id);
        }

        [Fact]
        public async Task ClosePedido_ShouldChangeOrderStatus()
        {
            var context = GetInMemoryDbContext();
            var repository = new PedidoRepository(context);
            var pedidoId = Guid.NewGuid();
            var pedido = new Pedido { Id = pedidoId, Status = StatusPedido.Aberto };

            var produto = new Produto { Id = Guid.NewGuid(), Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };
            var itemPedido = new ItemPedido
            {
                Id = Guid.NewGuid(),
                Produto = produto,
                Quantidade = 1
            };

            pedido.ItensPedidos.Add(itemPedido);

            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();

            var result = await repository.ClosePedido(pedidoId);
            Assert.Equal(StatusPedido.Fechado, result.Status);
        }

        [Fact]
        public async Task DeletePedido_ShouldRemoveOrder()
        {
            var context = GetInMemoryDbContext();
            var repository = new PedidoRepository(context);
            var pedidoId = Guid.NewGuid();
            var pedido = new Pedido { Id = pedidoId, Status = StatusPedido.Aberto };

            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();

            await repository.DeletePedido(pedidoId);
            var deletedPedido = await context.Pedidos.FindAsync(pedidoId);
            Assert.Null(deletedPedido);
        }
    }
}
