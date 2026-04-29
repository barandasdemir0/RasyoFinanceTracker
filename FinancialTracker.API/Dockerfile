# API Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Projeyi kopyala ve restore et
COPY ["FinancialTracker.API.csproj", "./"]
RUN dotnet restore "./FinancialTracker.API.csproj"

# Kalan tüm dosyaları kopyala ve build et
COPY . .
RUN dotnet publish "FinancialTracker.API.csproj" -c Release -o /app/publish

# Çalıştırma aşaması (Sadece runtime, SDK değil. Bu imajın boyutunu çok küçültür)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# 8080 portunu dışa aç
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinancialTracker.API.dll"]
