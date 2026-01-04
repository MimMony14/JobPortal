using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JobPortal.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

namespace JobPortal.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _env;

    public HomeController(IWebHostEnvironment env)
    {
        _env = env;
    }
    public IActionResult Index()
    {
        string filesPath = Path.Combine(_env.WebRootPath, "files");

        if (!Directory.Exists(filesPath))
        {
            return Content("Folder not found: " + filesPath);
        }

        List<string> pdfNames = Directory
            .GetFiles(filesPath, "*.pdf")
            .Select(Path.GetFileName)
            .ToList();

        return View(pdfNames);
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
    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}