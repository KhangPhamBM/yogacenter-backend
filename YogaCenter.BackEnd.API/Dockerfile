#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["YogaCenter.BackEnd.API/YogaCenter.BackEnd.API.csproj", "YogaCenter.BackEnd.API/"]
COPY ["YogaCenter.BackEnd.Service/YogaCenter.BackEnd.Service.csproj", "YogaCenter.BackEnd.Service/"]
COPY ["YogaCenter.BackEnd.DAL/YogaCenter.BackEnd.DAL.csproj", "YogaCenter.BackEnd.DAL/"]
RUN dotnet restore "YogaCenter.BackEnd.API/YogaCenter.BackEnd.API.csproj"
COPY . .
WORKDIR "/src/YogaCenter.BackEnd.API"
RUN dotnet build "YogaCenter.BackEnd.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YogaCenter.BackEnd.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YogaCenter.BackEnd.API.dll"]