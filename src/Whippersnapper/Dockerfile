#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Whippersnapper/Whippersnapper.csproj", "src/Whippersnapper/"]
RUN dotnet restore "src/Whippersnapper/Whippersnapper.csproj"
COPY . .
WORKDIR "/src/src/Whippersnapper"
RUN dotnet build "Whippersnapper.csproj" -c Release -o /app/build

FROM build AS publish
ENV GgmlRuntime=cpu
RUN dotnet publish "Whippersnapper.csproj" -c Release -o /app/publish /p:UseAppHost=true /p:GgmlRuntime=${GgmlRuntime}

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Whippersnapper.dll"]