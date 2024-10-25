# Use a imagem base do .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use a imagem do SDK do .NET 8 para construir o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FinancialManagementApp.csproj", "./"]
RUN dotnet restore "./FinancialManagementApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FinancialManagementApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinancialManagementApp.csproj" -c Release -o /app/publish

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialManagementApp.dll"]