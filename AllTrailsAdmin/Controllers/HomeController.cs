using AllTrailsAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AllTrailsAdmin.Controllers
{
    public class HomeController : Controller
    {
        // Action Method /Index
        public IActionResult Index()
        {
            //foreach (var c in User.Claims)
            //    Console.WriteLine($"{c.Type} = {c.Value}");

            return View();
        }

        // Action Method /Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
