# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the CSPROJ file and restore any dependencies (via NUGET)
COPY ["Cards.Api/Cards.Api.csproj", "Cards.Api/"]
COPY ["Cards.Application/Cards.Application.csproj", "Cards.Application/"]
COPY ["Cards.Domain/Cards.Domain.csproj", "Cards.Domain/"]
COPY ["Cards.Infrastructure/Cards.Infrastructure.csproj", "Cards.Infrastructure/"]
RUN dotnet restore "Cards.Api/Cards.Api.csproj"

# Copy the project files and build the release
COPY . ./
RUN dotnet publish "Cards.Api/Cards.Api.csproj" -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Cards.Api.dll"]

# The application will be running on port 8080
EXPOSE 8080
