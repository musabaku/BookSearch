# Use the official .NET image from the Docker Hub
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BookSearch/BookSearch.csproj", "BookSearch/"]
RUN dotnet restore "BookSearch/BookSearch.csproj"
COPY . .
WORKDIR "/src/BookSearch"
RUN dotnet build "BookSearch.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookSearch.csproj" -c Release -o /app/publish

# Final stage to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookSearch.dll"]
