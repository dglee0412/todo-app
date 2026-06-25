# ---- 1단계: React 빌드 ----
FROM node:22 AS frontend-build
WORKDIR /frontend
COPY frontend/package*.json ./
RUN npm install
COPY frontend/ ./
RUN npm run build

# ---- build 스테이지: SDK로 빌드/publish ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY TodoApi/TodoApi.csproj ./TodoApi/
RUN dotnet restore ./TodoApi/TodoApi.csproj
COPY TodoApi/ ./TodoApi/
# React 빌드 결과(dist)를 wwwroot로 복사
COPY --from=frontend-build /frontend/dist ./TodoApi/wwwroot
RUN dotnet publish ./TodoApi/TodoApi.csproj -c Release -o /app

# ---- runtime 스테이지: 가벼운 런타임 이미지 ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "TodoApi.dll"]
