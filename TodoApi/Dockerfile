# ---- build 스테이지: SDK로 빌드/publish ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY TodoApi.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app

# ---- runtime 스테이지: 가벼운 런타임 이미지 ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "TodoApi.dll"]
