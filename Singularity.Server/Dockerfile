#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Singularity.Server/Singularity.Server.csproj", "Singularity.Server/"]
RUN dotnet restore "Singularity.Server/Singularity.Server.csproj"
COPY . .
WORKDIR "/src/Singularity.Server"
RUN dotnet build "Singularity.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Singularity.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Singularity.Server.dll"]