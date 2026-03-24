# Multi-stage build for .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["LoanService.csproj", "./"]
RUN dotnet restore "LoanService.csproj"

# Copy everything and publish
COPY . .
RUN dotnet publish "LoanService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "LoanService.dll"]
