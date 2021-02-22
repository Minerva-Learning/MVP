#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Lean.Web.Host/Lean.Web.Host.csproj", "src/Lean.Web.Host/"]
COPY ["src/Lean.Web.Core/Lean.Web.Core.csproj", "src/Lean.Web.Core/"]
COPY ["src/Lean.Application/Lean.Application.csproj", "src/Lean.Application/"]
COPY ["src/Lean.Application.Shared/Lean.Application.Shared.csproj", "src/Lean.Application.Shared/"]
COPY ["src/Lean.Core.Shared/Lean.Core.Shared.csproj", "src/Lean.Core.Shared/"]
COPY ["src/Lean.Core/Lean.Core.csproj", "src/Lean.Core/"]
COPY ["src/Lean.EntityFrameworkCore/Lean.EntityFrameworkCore.csproj", "src/Lean.EntityFrameworkCore/"]
COPY ["src/Lean.GraphQL/Lean.GraphQL.csproj", "src/Lean.GraphQL/"]
RUN dotnet restore "src/Lean.Web.Host/Lean.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/Lean.Web.Host"
RUN dotnet build "Lean.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lean.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lean.Web.Host.dll"]
