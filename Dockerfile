FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY ["Million.API/Million.API.csproj", "Million.API/"]
COPY ["Million.Application/Million.Application.csproj", "Million.Application/"]
COPY ["Million.Domain/Million.Domain.csproj", "Million.Domain/"]
COPY ["Million.Infrastructure/Million.Infrastructure.csproj", "Million.Infrastructure/"]

RUN dotnet restore "Million.API/Million.API.csproj"

COPY . .

RUN dotnet publish "Million.API/Million.API.csproj" -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build-env /out .

ENTRYPOINT ["dotnet", "Million.API.dll"]

EXPOSE 80
