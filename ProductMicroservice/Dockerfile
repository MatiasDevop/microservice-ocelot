#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

EXPOSE 443
EXPOSE 80

# copy project csproj file and restore it in docker directory
COPY ./*.csproj ./
RUN dotnet restore

# Copy everything into the docker directory and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime final image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ProductMicroservice.dll"]

