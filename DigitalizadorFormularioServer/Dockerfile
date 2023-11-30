FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DigitalizadorFormularioServer.csproj", "."]
RUN dotnet restore "DigitalizadorFormularioServer.csproj"
COPY . .
RUN dotnet build "DigitalizadorFormularioServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DigitalizadorFormularioServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigitalizadorFormularioServer.dll"]