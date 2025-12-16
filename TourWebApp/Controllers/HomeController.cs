using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TourWebApp.Models;
using TourWebApp.Data.Models;
using TourWebApp.Data;       

namespace TourWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;   

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        
       var tourNoiBat = _context.Tours
        .Where(t => t.TrangThai == true)
        .OrderByDescending(t => t.SoNguoiDaDat)
        .Take(3)
        .ToList();

        ViewBag.TourNoiBat = tourNoiBat;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel 
        { 
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
        });
    }
}
