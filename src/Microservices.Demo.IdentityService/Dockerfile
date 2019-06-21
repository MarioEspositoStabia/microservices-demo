FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Microservices.Demo.IdentityService/Microservices.Demo.IdentityService.csproj", "src/Microservices.Demo.IdentityService/"]
RUN dotnet restore "src/Microservices.Demo.IdentityService/Microservices.Demo.IdentityService.csproj"
COPY . .
WORKDIR "/src/src/Microservices.Demo.IdentityService"
RUN dotnet build "Microservices.Demo.IdentityService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Microservices.Demo.IdentityService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Microservices.Demo.IdentityService.dll"]