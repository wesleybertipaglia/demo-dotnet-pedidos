using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Pedidos.Domain.Entities;
using Pedidos.Infrastructure.Data;
using Pedidos.Infrastructure.Repositories;

namespace Pedidos.Tests
{
    public class ProdutoRepositoryTests
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
        public async Task GetAllProdutos_ShouldReturnAllProducts()
        {
            var context = GetInMemoryDbContext();
            var repository = new ProdutoRepository(context);

            context.Produtos.Add(new Produto { Id = Guid.NewGuid(), Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f });
            context.Produtos.Add(new Produto { Id = Guid.NewGuid(), Titulo = "Produto 2", Descricao = "Descrição 2", Preco = 20.0f });
            await context.SaveChangesAsync();

            var result = await repository.GetAllProdutos(1, 10, null, null, null);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(2, result.Produtos.Count());
        }

        [Fact]
        public async Task GetProdutoById_ShouldReturnProduct()
        {
            var context = GetInMemoryDbContext();
            var repository = new ProdutoRepository(context);
            var produtoId = Guid.NewGuid();

            context.Produtos.Add(new Produto { Id = produtoId, Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f });
            await context.SaveChangesAsync();

            var result = await repository.GetProdutoById(produtoId);
            Assert.NotNull(result);
            Assert.Equal(produtoId, result.Id);
        }

        [Fact]
        public async Task CreateProduto_ShouldAddProduct()
        {
            var context = GetInMemoryDbContext();
            var repository = new ProdutoRepository(context);
            var newProduto = new Produto { Id = Guid.NewGuid(), Titulo = "Novo Produto", Descricao = "Nova Descrição", Preco = 30.0f };

            var result = await repository.CreateProduto(newProduto);
            var produto = await context.Produtos.FindAsync(newProduto.Id);
            Assert.NotNull(produto);
            Assert.Equal(newProduto.Titulo, produto.Titulo);
        }

        [Fact]
        public async Task UpdateProduto_ShouldModifyProduct()
        {
            var context = GetInMemoryDbContext();
            var repository = new ProdutoRepository(context);
            var produtoId = Guid.NewGuid();
            var produto = new Produto { Id = produtoId, Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };

            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            produto.Titulo = "Produto Atualizado";
            produto.Descricao = "Descrição Atualizada";
            var result = await repository.UpdateProduto(produto);

            var updatedProduto = await context.Produtos.FindAsync(produtoId);
            Assert.Equal("Produto Atualizado", updatedProduto.Titulo);
            Assert.Equal("Descrição Atualizada", updatedProduto.Descricao);
        }

        [Fact]
        public async Task DeleteProduto_ShouldRemoveProduct()
        {
            var context = GetInMemoryDbContext();
            var repository = new ProdutoRepository(context);
            var produtoId = Guid.NewGuid();
            var produto = new Produto { Id = produtoId, Titulo = "Produto 1", Descricao = "Descrição 1", Preco = 10.0f };

            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            await repository.DeleteProduto(produtoId);
            var deletedProduto = await context.Produtos.FindAsync(produtoId);
            Assert.Null(deletedProduto);
        }
    }
}
