using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogManagement.Models;

namespace BlogManagement.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {}

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
