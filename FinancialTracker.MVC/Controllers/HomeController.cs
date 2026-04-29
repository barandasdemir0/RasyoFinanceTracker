using FinancialTracker.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinancialTracker.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
