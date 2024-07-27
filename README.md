# API de Pedidos

API .NET de pedidos simples implementada usando os principios do Domain-Driven Design (DDD), construida com .NET Core, Entity Framework Core, xUnit, e Docker.

## Sumário

- [Primeiros passos](#primeiros-passos)
- [Recursos](#recursos)
- [Diagrama de Entidade e Relacionamento](#diagrama-entidade-relacionamento)
- [Endpoints](#endpoints)
- [Licença](#licença)

## Primeiros passos

### Executando com Docker

A maneira mais fácil de executar a aplicação é com Docker.

```bash
docker-compose up -d
```

> A aplicação estará disponível em [https://localhost:5000](https://localhost:5000).

### Executando com .NET CLI

Como alternativa, você pode executar a aplicação com a CLI do .NET.

#### Pré-requisitos

- [.NET Core](https://dotnet.microsoft.com/download)

```bash
dotnet run --project API/API.csproj
```

> A aplicação estará disponível em [https://localhost:5000](https://localhost:5000).

## Recursos

O aplicativo inclui as seguintes funcionalidades:

- Arquitetura limpa e DDD
- Entity Framework Core
- Entity Framework Core InMemory
- Testes unitários com xUnit
- Swagger UI
- Dockerfile and docker-compose.yml

## Diagrama Entidade Relacionamento

O seguinte Diagrama Entidade-Relacionamento (DER) mostra os relacionamentos entre as entidades na aplicação:

```mermaid
classDiagram
    class Produto {
        Guid Id
        string Titulo
        string Descricao
        float Preco
    }

    class Pedido {
        Guid Id
        StatusPedido Status
    }

    class ItemPedido {
        Guid Id        
        int Quantidade
        float Preco
        Produto Produto
        Pedido Pedido
    }

    Pedido "1" --> "n" ItemPedido : contém
    ItemPedido "1" --> "1" Produto : refere-se
```

## Endpoints

### Produtos

- `POST /api/Produtos`: Adiciona um novo produto.
- `GET /api/Produtos`: Retorna todos os produtos.
- `GET /api/Produtos/{id}`: Retorna um produto por ID.
- `PUT /api/Produtos/{id}`: Atualiza um produto por ID.
- `DELETE /api/Produtos/{id}`: Deleta um produto por ID.

### Pedidos

- `POST /api/Pedidos`: Adiciona um novo pedido.
- `GET /api/Pedidos`: Retorna todos os pedidos.
- `GET /api/Pedidos/{id}`: Retorna um pedido por ID.
- `DELETE /api/Pedidos/{id}`: Deleta um pedido por ID.
- `POST /api/Pedidos/{id}/close`: Fecha um pedido.

### Itens do Pedido

- `POST /api/Pedidos/{id}/itens`: Adiciona um item ao pedido.
- `PUT /api/Pedidos/{id}/itens`: Atualiza um item do pedido.
- `DELETE /api/Pedidos/{id}/itens`: Deleta um item do pedido.

## Licença

Este projeto está licenciado sob a licença MIT - consulte o arquivo [LICENSE](./LICENSE) para obter detalhes.
