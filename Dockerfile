# Use the official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5056

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FooBar.csproj", "./"]
RUN dotnet restore "./FooBar.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "FooBar.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FooBar.csproj" -c Release -o /app/publish

# Final stage: build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FooBar.dll"]