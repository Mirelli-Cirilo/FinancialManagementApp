# Use uma imagem oficial do .NET como base
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "FinancialManagementApp.csproj"
RUN dotnet build "FinancialManagementApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinancialManagementApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialManagementApp.dll"]