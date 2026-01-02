using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JobPortal.Models;

namespace JobPortal.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Admin()
    {
        return View();
    }
    public IActionResult Applicant()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Index(string Name)
    {
        if(Name == "Lamia" || Name == "lamia")
        {
            return RedirectToAction("Admin");
        }
        else if(Name == "job")
        {
            return RedirectToAction("Index","Jobs"); //redirected to JobsController
        }
        else if(Name == "org")
        {
            return RedirectToAction("OrgDash","Admin"); //redirected to AdminController
        }
        else
        {
            return RedirectToAction("Dashboard", "Admin");
        }
    }
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
