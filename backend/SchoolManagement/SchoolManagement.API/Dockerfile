# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["SchoolManagement.API.csproj", "./"]

RUN dotnet restore "SchoolManagement.API.csproj"

COPY . .

RUN dotnet publish "SchoolManagement.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "SchoolManagement.API.dll"]