FROM microsoft/dotnet:2.2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2.104-sdk AS build
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