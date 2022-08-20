FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /cert
RUN dotnet dev-certs https -ep hasmartapi.pfx -p c3rtp455w0rd

WORKDIR /app

# copy csproj and restore as distinct layers
COPY HASmart.WebApi/*.csproj ./HASmart.WebApi/
COPY HASmart.Infrastructure/*.csproj ./HASmart.Infrastructure/
COPY HASmart.Core/*.csproj ./HASmart.Core/
COPY HASmart.Test/*.csproj ./HASmart.Test/
COPY HASmart.sln .
# WORKDIR /app/dotnetapp
RUN dotnet restore

# copy and publish app and libraries
# WORKDIR /app/
COPY HASmart.WebApi/. ./HASmart.WebApi/
COPY HASmart.Infrastructure/. ./HASmart.Infrastructure/
COPY HASmart.Core/. ./HASmart.Core/
COPY HASmart.Test/. ./HASmart.Test/
# WORKDIR /app/HASmart.WebApi
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /certs/https
COPY --from=build /cert/hasmartapi.pfx ./

WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "HASmart.WebApi.dll"]