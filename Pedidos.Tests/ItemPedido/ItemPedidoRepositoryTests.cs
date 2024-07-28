using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Enums;
using Pedidos.Infrastructure.Data;
using Pedidos.Infrastructure.Exceptions;
using Pedidos.Infrastructure.Repositories;

namespace Pedidos.Tests
{
    public class ItemPedidoRepositoryTests
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
        public async Task AddItemPedido_ShouldAddNewItemToPedido()
        {
            var context = GetInMemoryDbContext();
            var repository = new ItemPedidoRepository(context);

            var pedido = new Pedido { Id = Guid.NewGuid(), Status = StatusPedido.Aberto };
            var produto = new Produto { Id = Guid.NewGuid(), Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };

            context.Pedidos.Add(pedido);
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            var result = await repository.AddItemPedido(pedido.Id, produto.Id, 2);
            Assert.Single(result.ItensPedidos);
            Assert.Equal(2, result.ItensPedidos.First().Quantidade);
        }

        [Fact]
        public async Task AddItemPedido_ShouldIncreaseQuantityIfItemAlreadyExists()
        {
            var context = GetInMemoryDbContext();
            var repository = new ItemPedidoRepository(context);

            var pedido = new Pedido { Id = Guid.NewGuid(), Status = StatusPedido.Aberto };
            var produto = new Produto { Id = Guid.NewGuid(), Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };
            var itemPedido = new ItemPedido(1, produto.Preco, produto, pedido);

            context.Pedidos.Add(pedido);
            context.Produtos.Add(produto);
            context.ItensPedidos.Add(itemPedido);
            await context.SaveChangesAsync();

            var result = await repository.AddItemPedido(pedido.Id, produto.Id, 2);
            Assert.Single(result.ItensPedidos);
            Assert.Equal(3, result.ItensPedidos.First().Quantidade);
        }

        [Fact]
        public async Task AddItemPedido_ShouldThrowExceptionForClosedPedido()
        {
            var context = GetInMemoryDbContext();
            var repository = new ItemPedidoRepository(context);

            var pedido = new Pedido { Id = Guid.NewGuid(), Status = StatusPedido.Fechado };
            var produto = new Produto { Id = Guid.NewGuid(), Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };

            context.Pedidos.Add(pedido);
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            await Assert.ThrowsAsync<BadRequestException>(() => repository.AddItemPedido(pedido.Id, produto.Id, 2));
        }

        [Fact]
        public async Task RemoveItemPedido_ShouldRemoveItemFromPedido()
        {
            var context = GetInMemoryDbContext();
            var repository = new ItemPedidoRepository(context);

            var pedido = new Pedido { Id = Guid.NewGuid(), Status = StatusPedido.Aberto };
            var produto = new Produto { Id = Guid.NewGuid(), Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };
            var itemPedido = new ItemPedido(1, produto.Preco, produto, pedido);

            context.Pedidos.Add(pedido);
            context.Produtos.Add(produto);
            context.ItensPedidos.Add(itemPedido);
            await context.SaveChangesAsync();

            var result = await repository.RemoveItemPedido(pedido.Id, produto.Id);
            Assert.Empty(result.ItensPedidos);
        }

        [Fact]
        public async Task RemoveItemPedido_ShouldThrowExceptionForClosedPedido()
        {
            var context = GetInMemoryDbContext();
            var repository = new ItemPedidoRepository(context);

            var pedido = new Pedido { Id = Guid.NewGuid(), Status = StatusPedido.Fechado };
            var produto = new Produto { Id = Guid.NewGuid(), Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };
            var itemPedido = new ItemPedido(1, produto.Preco, produto, pedido);

            context.Pedidos.Add(pedido);
            context.Produtos.Add(produto);
            context.ItensPedidos.Add(itemPedido);
            await context.SaveChangesAsync();

            await Assert.ThrowsAsync<BadRequestException>(() => repository.RemoveItemPedido(pedido.Id, produto.Id));
        }

        [Fact]
        public async Task UpdateItemPedido_ShouldUpdateQuantity()
        {
            var context = GetInMemoryDbContext();
            var repository = new ItemPedidoRepository(context);

            var pedido = new Pedido { Id = Guid.NewGuid(), Status = StatusPedido.Aberto };
            var produto = new Produto { Id = Guid.NewGuid(), Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };
            var itemPedido = new ItemPedido(1, produto.Preco, produto, pedido);

            context.Pedidos.Add(pedido);
            context.Produtos.Add(produto);
            context.ItensPedidos.Add(itemPedido);
            await context.SaveChangesAsync();

            var result = await repository.UpdateItemPedido(pedido.Id, produto.Id, 5);
            Assert.Equal(5, result.ItensPedidos.First().Quantidade);
        }

        [Fact]
        public async Task UpdateItemPedido_ShouldThrowExceptionForClosedPedido()
        {
            var context = GetInMemoryDbContext();
            var repository = new ItemPedidoRepository(context);

            var pedido = new Pedido { Id = Guid.NewGuid(), Status = StatusPedido.Fechado };
            var produto = new Produto { Id = Guid.NewGuid(), Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };
            var itemPedido = new ItemPedido(1, produto.Preco, produto, pedido);

            context.Pedidos.Add(pedido);
            context.Produtos.Add(produto);
            context.ItensPedidos.Add(itemPedido);
            await context.SaveChangesAsync();

            await Assert.ThrowsAsync<BadRequestException>(() => repository.UpdateItemPedido(pedido.Id, produto.Id, 5));
        }
    }
}
