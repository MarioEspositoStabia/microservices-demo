FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Micorservices.Demo.ApiGateway/Micorservices.Demo.ApiGateway.csproj", "src/Micorservices.Demo.ApiGateway/"]
RUN dotnet restore "src/Micorservices.Demo.ApiGateway/Micorservices.Demo.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/Micorservices.Demo.ApiGateway"
RUN dotnet build "Micorservices.Demo.ApiGateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Micorservices.Demo.ApiGateway.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Micorservices.Demo.ApiGateway.dll"]