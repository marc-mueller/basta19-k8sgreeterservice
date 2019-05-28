FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Greeter.Service/Greeter.Service.csproj", "Greeter.Service/"]
RUN dotnet restore "Greeter.Service/Greeter.Service.csproj"
COPY . .
WORKDIR "/src/Greeter.Service"
RUN dotnet build "Greeter.Service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Greeter.Service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Greeter.Service.dll"]