#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ZingTickets/ZingTickets.API/ZingTickets.API.csproj", "ZingTickets/ZingTickets.API/"]
RUN dotnet restore "ZingTickets/ZingTickets.API/ZingTickets.API.csproj"
COPY . .
WORKDIR "/src/ZingTickets/ZingTickets.API"
RUN dotnet build "ZingTickets.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZingTickets.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZingTickets.API.dll"]