FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY OrderProcessingService.sln ./
COPY OrderProcessingService/OrderProcessingService.csproj src/OrderProcessingService/
COPY OrderProcessingService.DAL/OrderProcessingService.DAL.csproj src/OrderProcessingService.DAL/
RUN dotnet restore src/OrderProcessingService/OrderProcessingService.csproj

COPY ./ ./src/

RUN dotnet publish src/OrderProcessingService/OrderProcessingService.csproj \
    -c Release -o /app/publish -r linux-musl-x64 --self-contained false /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "OrderProcessingService.dll"]