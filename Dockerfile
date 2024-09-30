# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory in the container
WORKDIR /app

# Copy the csproj file and restore dependencies
COPY ["Million.API/Million.API.csproj", "Million.API/"]
COPY ["Million.Application/Million.Application.csproj", "Million.Application/"]
COPY ["Million.Domain/Million.Domain.csproj", "Million.Domain/"]
COPY ["Million.Infrastructure/Million.Infrastructure.csproj", "Million.Infrastructure/"]

# Restore the dependencies
RUN dotnet restore "Million.API/Million.API.csproj"

# Copy the rest of the application code
COPY . .

# Publish the app in release mode to the /out directory
RUN dotnet publish "Million.API/Million.API.csproj" -c Release -o /out

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published files from the build environment
COPY --from=build-env /out .

# Set the entry point to run the API
ENTRYPOINT ["dotnet", "Million.API.dll"]

# Expose the port the API will run on
EXPOSE 80
