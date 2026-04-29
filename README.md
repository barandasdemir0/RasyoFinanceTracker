# Rasyonet - Financial Data Tracker API & MVC Dashboard

## 📌 Project Purpose
This project is a complete ecosystem containing a RESTful Web API and a real-time MVC Dashboard designed for tracking personal financial portfolios and market watchlists. It was developed as part of the **Rasyonet Software Engineering Internship Technical Assessment**. 

The application allows users to:
- Add assets (Stocks, Cryptos, etc.) with their quantity and average cost.
- Add assets to a "Watchlist" (Quantity = 0) to monitor target prices.
- Automatically fetch real-time prices from external financial markets in the background.
- Calculate total portfolio value and P&L (Profit/Loss).
- Generate "Radar Alarms" when an asset's market price approaches the user's target price.
- View real-time updates seamlessly on a premium custom-styled web interface without manual refreshing.

## 🛠️ Technology Stack & Choices
- **Backend Framework:** .NET 10 Web API
- **Frontend Framework:** ASP.NET Core MVC (Razor Pages) with pure CSS (OKLCH Color Space)
- **Language:** C# 14
- **Real-time Communication:** SignalR
- **Database:** Microsoft SQL Server (via Entity Framework Core 10)
- **Validation:** FluentValidation
- **Testing:** xUnit
- **API Documentation:** Scalar OpenAPI (Modern UI natively supported in .NET 10)

### 📊 External API Choice: Finnhub
I chose **Finnhub** (`finnhub.io`) because it provides real-time quote data with a generous free tier (60 requests/minute). It seamlessly supports both standard stock tickers (e.g., `AAPL`) and cryptocurrency tickers (e.g., `BINANCE:BTCUSDT`), fitting perfectly with the polymorphic nature of the `AssetType` enum.

---

## 🏗️ Architecture & Design Patterns

The project follows **Clean Architecture** principles, strictly separating concerns across layers (Controllers -> Services -> Repositories -> External Providers). 

We also strictly follow the **"Smart Backend, Dumb Frontend"** philosophy. The MVC frontend contains zero business logic; it acts purely as a presentation layer that renders the pre-calculated view models supplied by the API.

### 1. Strategy Pattern (`IFinancialDataProvider`)
**Where:** `Services/External/FinnhubDataProvider.cs`
**Why:** The Strategy Pattern abstracts the implementation of fetching financial data. By depending on `IFinancialDataProvider`, the application is decoupled from Finnhub. If we migrate to *Alpha Vantage* tomorrow, we only need to implement a new class and change a single line in the DI container.

### 2. Repository Pattern (`ITrackedAssetRepository`)
**Where:** `Repositories/TrackedAssetRepository.cs`
**Why:** It acts as a collection of domain objects in memory, abstracting the `AppDbContext` and EF Core specifics away from the Business layer (`PortfolioService`). This prevents database-specific code from leaking into the domain.

### 3. Observer Pattern (`DashboardHub`)
**Where:** `Hubs/DashboardHub.cs` & `Services/PriceRefreshBackgroundService.cs`
**Why:** Used via SignalR to push real-time updates to connected clients. Instead of the browser constantly polling the server (which wastes resources), the `BackgroundService` acts as the Subject, notifying all connected Observers (browsers) exactly when new price data is fetched from Finnhub, prompting them to elegantly refresh their views.

---

## ⚖️ Trade-offs & Engineering Decisions

1. **Manual Mapping vs. AutoMapper:** I deliberately chose to use Manual Mapping via Extension Methods (`AssetMappingExtensions.cs`) instead of AutoMapper. AutoMapper relies heavily on reflection, which impacts performance. Manual mapping provides **compile-time safety** and faster execution.
2. **Background Polling Rate:** The `PriceRefreshBackgroundService` polls Finnhub every 60 seconds. This is a deliberate trade-off to respect Finnhub's free tier rate limit (60 req/min). In a production environment with a paid API, this could be reduced to milliseconds.
3. **Hardcoded Chart Data:** The line chart on the dashboard currently uses sample data for visualization purposes. Since the database only collects snapshots while the application is running, generating a meaningful 7-day historical chart requires days of data collection. In a real-world scenario, this would be fueled by aggregating the `PriceSnapshot` table history.
4. **Hard Delete vs. Soft Delete:** I implemented a "Hard Delete" for removing assets. Since this is a personal tracker, keeping clutter in the database is unnecessary. In a true enterprise banking system, I would have implemented a "Soft Delete" using EF Core Global Query Filters for audit trails.

---

## 🧪 Testing
The project includes meaningful Unit Tests targeting the core business logic.
- **`AssetMappingExtensionsTests`**: Verifies that the mathematical logic for calculating Profit/Loss (P&L), Total Value, and Target Proximity (Radar Alerts) works flawlessly independent of any database or external service.
