#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PRN231_Group7.Assignment2.API/PRN231_Group7.Assignment2.API.csproj", "PRN231_Group7.Assignment2.API/"]
COPY ["PRN231_Group7.Assignment2.Contract/PRN231_Group7.Assignment2.Contract.csproj", "PRN231_Group7.Assignment2.Contract/"]
COPY ["PRN231_Group7.Assignment2.Repo/PRN231_Group7.Assignment2.Repo.csproj", "PRN231_Group7.Assignment2.Repo/"]
RUN dotnet restore "./PRN231_Group7.Assignment2.API/PRN231_Group7.Assignment2.API.csproj"
COPY . .
WORKDIR "/src/PRN231_Group7.Assignment2.API"
RUN dotnet build "./PRN231_Group7.Assignment2.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PRN231_Group7.Assignment2.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRN231_Group7.Assignment2.API.dll"]