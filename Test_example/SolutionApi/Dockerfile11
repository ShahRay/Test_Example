#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 870


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Test_example/Test_example.csproj", "Test_example/"]
COPY ["Test_example.Service/Test_example.Service.csproj", "Test_example.Service/"]
COPY ["Test_example.Messaging/Test_example.Messaging.csproj", "Test_example.Messaging/"]
COPY ["Test_example.Domain/Test_example.Domain.csproj", "Test_example.Domain/"]
RUN dotnet restore "Test_example/Test_example.csproj"
COPY . .
WORKDIR "/src/Test_example"
RUN dotnet build "Test_example.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test_example.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test_example.dll"]