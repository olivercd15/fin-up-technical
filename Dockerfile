FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Payments.WebApi/ Payments.WebApi/
COPY Payments.Application/ Payments.Application/
COPY Payments.Infrastructure/ Payments.Infrastructure/
COPY Payments.Domain/ Payments.Domain/

RUN dotnet restore "Payments.WebApi/Payments.WebApi.csproj"
RUN dotnet build "Payments.WebApi/Payments.WebApi.csproj" -c Release -o /app/build
RUN dotnet publish "Payments.WebApi/Payments.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Payments.WebApi.dll"]
