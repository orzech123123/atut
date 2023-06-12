FROM mcr.microsoft.com/dotnet/sdk:2.1 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:2.1
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Atut.dll"]