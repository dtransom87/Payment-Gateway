FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8080
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/payment-gateway.csproj", "./"]
RUN dotnet restore "./payment-gateway.csproj"
COPY . .
#WORKDIR "/src/payment-gateway."
RUN dotnet build "payment-gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "payment-gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "payment-gateway.dll"]