FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /stage
ARG ASPNETCORE_ENVIRONMENT="Production"
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT

COPY . .
RUN dotnet restore "SslCertificateChecker.csproj"
RUN dotnet publish "SslCertificateChecker.csproj" -c Release -o output --no-restore


#Server Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine
WORKDIR /prod
COPY --from=build /stage/output ./

ENTRYPOINT ["dotnet","SslCertificateChecker.dll"]
