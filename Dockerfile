FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.sln ./
COPY Server/Api/*.csproj ./Server/Api/
COPY Server/DataAccess/*.csproj ./Server/DataAccess/
COPY Server/Service/*.csproj ./Server/Service/
COPY Server/Tests/*.csproj ./Server/Tests/

RUN dotnet restore 

# copy everything else and build app
COPY Server/Api/. ./Server/Api/
COPY Server/DataAccess/. ./Server/DataAccess/
COPY Server/Service/. ./Server/Service/
COPY Server/Tests/. ./Server/Tests/
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Api.dll"]




