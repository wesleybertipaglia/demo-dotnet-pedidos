FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY Pedidos.API/Pedidos.API.csproj Pedidos.API/
COPY Pedidos.Application/Pedidos.Application.csproj Pedidos.Application/
COPY Pedidos.Domain/Pedidos.Domain.csproj Pedidos.Domain/
COPY Pedidos.Infrastructure/Pedidos.Infrastructure.csproj Pedidos.Infrastructure/

RUN dotnet restore

COPY . .
RUN dotnet build -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "Pedidos.API.dll"]