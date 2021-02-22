#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Lean.Migrator/Lean.Migrator.csproj", "src/Lean.Migrator/"]
COPY ["src/Lean.EntityFrameworkCore/Lean.EntityFrameworkCore.csproj", "src/Lean.EntityFrameworkCore/"]
COPY ["src/Lean.Core/Lean.Core.csproj", "src/Lean.Core/"]
COPY ["src/Lean.Core.Shared/Lean.Core.Shared.csproj", "src/Lean.Core.Shared/"]
RUN dotnet restore "src/Lean.Migrator/Lean.Migrator.csproj"
COPY . .
WORKDIR "/src/src/Lean.Migrator"
RUN dotnet build "Lean.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lean.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lean.Migrator.dll"]
