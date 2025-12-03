# ============================
# BUILD STAGE
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ code vào container
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish project ở chế độ Release
RUN dotnet publish -c Release -o /app/publish



# ============================
# RUNTIME STAGE
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy code đã publish từ stage build
COPY --from=build /app/publish .

# Render sẽ tự gán PORT, bạn cần expose 8080
EXPOSE 8080

# Chạy file DLL của project
ENTRYPOINT ["dotnet", "SneakerShop.dll"]
