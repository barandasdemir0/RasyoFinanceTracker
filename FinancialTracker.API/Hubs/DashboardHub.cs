using Microsoft.AspNetCore.SignalR;

namespace FinancialTracker.API.Hubs;

// DESIGN PATTERN: Observer Pattern
// WHY: SignalR Hub acts as a broadcast channel. When the BackgroundService detects 
// new price data, it notifies all connected browser clients without them polling.
public class DashboardHub:Hub
{
}
