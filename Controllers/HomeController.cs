using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JobPortal.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

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
    public IActionResult ViewPdf(string name)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files/"+name);
        if (!System.IO.File.Exists(path))
            return NotFound();
        return PhysicalFile(path, "application/pdf");
    }

    
    [HttpPost]
    public IActionResult Index(string Name)
    {
        if(Name == "Lamia" || Name == "lamia")
        {
            return RedirectToAction("Applicant");
        }
        else if(Name == "job")
        {
            return RedirectToAction("Index","Jobs"); //redirected to JobsController
        }
        else if(Name == "org")
        {
            return RedirectToAction("OrgDash","Organization"); //redirected to OrganizationController
        }
        else if(Name == "cv")
        {
            return RedirectToAction("ViewPdf", new { name = Name });
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