#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Test_Example.Worker.Back/Test_Example.Worker.Back.csproj", "Test_Example.Worker.Back/"]
COPY ["Test_Example.Back.Data/Test_Example.Back.Data.csproj", "Test_Example.Back.Data/"]
COPY ["Test_Example.Back.Domain/Test_Example.Back.Domain.csproj", "Test_Example.Back.Domain/"]
COPY ["Test_Example.Back.Messaging/Test_Example.Back.Messaging.csproj", "Test_Example.Back.Messaging/"]
COPY ["Test_Example.Back.Services/Test_Example.Back.Services.csproj", "Test_Example.Back.Services/"]
RUN dotnet restore "Test_Example.Worker.Back/Test_Example.Worker.Back.csproj"
COPY . .
WORKDIR "/src/Test_Example.Worker.Back"
RUN dotnet build "Test_Example.Worker.Back.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test_Example.Worker.Back.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test_Example.Worker.Back.dll"]