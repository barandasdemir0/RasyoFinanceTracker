# MVC Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["FinancialTracker.MVC.csproj", "./"]
RUN dotnet restore "./FinancialTracker.MVC.csproj"

COPY . .
RUN dotnet publish "FinancialTracker.MVC.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "FinancialTracker.MVC.dll"]
