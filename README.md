# RasyoFinanceTracker


# Rasyonet - Financial Data Tracker API

## 📌 Project Purpose
This project is a RESTful Web API designed for tracking personal financial portfolios and market watchlists. It was developed as part of the **Rasyonet Software Engineering Internship Technical Assessment**. 

The application allows users to:
- Add assets (Stocks, Cryptos, etc.) with their quantity and average cost.
- Add assets to a "Watchlist" (Quantity = 0) to monitor target prices.
- Automatically fetch real-time prices from external financial markets.
- Calculate total portfolio value and P&L (Profit/Loss).
- Generate "Radar Alarms" when an asset's market price approaches the user's target price.

## 🛠️ Technology Stack & Choices
- **Framework:** .NET 10 Web API
- **Language:** C# 14
- **Code Quality:** TreatWarningsAsErrors enabled (Zero-warning tolerance policy)
- **Database:** Microsoft SQL Server (via Entity Framework Core 10)
- **Validation:** FluentValidation (Manual invocation for better control)
- **API Documentation:** Scalar OpenAPI (Modern UI natively supported in .NET 10)


### 📊 External API Choice: Finnhub
I chose **Finnhub** (`finnhub.io`) because it provides real-time quote data with a generous free tier (60 requests/minute). It seamlessly supports both standard stock tickers (e.g., `AAPL`) and cryptocurrency tickers (e.g., `BINANCE:BTCUSDT`), fitting perfectly with the polymorphic nature of the `AssetType` enum.

### 💾 Database Choice: SQL Server & EF Core
I opted for **SQL Server** using Entity Framework Core's Code-First approach. SQL Server is robust and an industry standard for financial applications. Using EF Core allowed me to implement a pure Domain Entity model decoupled from the database schema.

---

## 🏗️ Architecture & Design Patterns

The project follows **Clean Architecture** principles, strictly separating concerns across layers (Controllers -> Services -> Repositories -> External Providers). Business logic is strictly kept inside the Service layer.

### 1. Strategy Pattern (`IFinancialDataProvider`)
**Where:** `Services/External/FinnhubDataProvider.cs`
**Why:** The Strategy Pattern abstracts the implementation of fetching financial data. By depending on `IFinancialDataProvider`, the application is decoupled from Finnhub. If the company decides to migrate to *Alpha Vantage* tomorrow, we only need to implement a new class and change a single line in the DI container, without touching any core business logic.

### 2. Repository Pattern (`ITrackedAssetRepository`)
**Where:** `Repositories/TrackedAssetRepository.cs`
**Why:** It acts as a collection of domain objects in memory, abstracting the `AppDbContext` and EF Core specifics away from the Business layer (`PortfolioService`). This makes the `PortfolioService` highly testable via mocking and prevents database-specific code from leaking into the domain.

---

## ⚖️ Trade-offs & Engineering Decisions

1. **Manual Mapping vs. AutoMapper:** I deliberately chose to use Manual Mapping via Extension Methods (`AssetMappingExtensions.cs`) instead of AutoMapper. AutoMapper relies heavily on reflection, which impacts performance. Manual mapping provides **compile-time safety** and faster execution, which is crucial for modern, highly-performant .NET applications.
2. **Hard Delete vs. Soft Delete:** I implemented a "Hard Delete" for removing assets. Since this is a personal tracker, keeping clutter in the database is unnecessary. In a true enterprise banking system, I would have implemented a "Soft Delete" using EF Core Global Query Filters for audit trails.
3. **N+1 API Problem & Rate Limiting:** In the `RefreshPricesAsync` method, I utilized `Task.WhenAll` to fetch prices concurrently instead of a sequential `foreach` loop. This significantly reduces execution time. However, to respect Finnhub's 60 req/min rate limit in a production environment with thousands of users, a batching or queuing mechanism (like RabbitMQ) should be introduced.

---
