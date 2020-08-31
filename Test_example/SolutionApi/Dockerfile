#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 717

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["SolutionApi/Test_example/Test_example.csproj", "SolutionApi/Test_example/"]
COPY ["SolutionApi/Test_example.Service/Test_example.Service.csproj", "SolutionApi/Test_example.Service/"]
COPY ["SolutionApi/Test_example.Messaging/Test_example.Messaging.csproj", "SolutionApi/Test_example.Messaging/"]
COPY ["SolutionApi/Test_example.Domain/Test_example.Domain.csproj", "SolutionApi/Test_example.Domain/"]
RUN dotnet restore "SolutionApi/Test_example/Test_example.csproj"
COPY . .
WORKDIR "/src/SolutionApi/Test_example"
RUN dotnet build "Test_example.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test_example.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test_example.dll"]