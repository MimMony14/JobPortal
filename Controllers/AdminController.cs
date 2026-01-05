using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobPortal.Services;
using JobPortal.Data;     
using JobPortal.Models;   
public class AdminController : Controller
{

    private readonly JobPortalContext _context;
    

    private readonly EmailService _email;
    public AdminController(JobPortalContext context , EmailService email)
    {
        _context = context;
         _email = email;
    }
    public IActionResult Dashboard()
    {
        ViewBag.Organizations = new SelectList(
            _context.Organizations.ToList(),
            "OrganizationId",
            "OrganizationName"
        );
        var jobs = _context.Jobs.Include(j => j.Organization).ToList();

        ViewBag.TotalJobs = jobs.Count();
        ViewBag.TotalOrganizations = _context.Organizations.Count();

        // You do not have applications table yet, so vacancy is the only honest proxy
        ViewBag.TotalApplications = jobs.Sum(j => j.Vacancy ?? 0);
        ViewBag.TotalApplicants = jobs.Sum(j => j.Vacancy ?? 0);

        ViewBag.RecentJobs = jobs
            .OrderByDescending(j => j.CreateDate)
            .Take(4)
            .Select(j => new
            {
                Title = j.Title,
                //Company = j.Organization.OrganizationName
                Company = j.Organization?.OrganizationName ?? "N/A"

,
                Time = j.CreateDate
            })
            .ToList();

        ViewBag.ChartLabels = jobs
            .Where(j => j.CreateDate != null)
            .GroupBy(j => j.CreateDate.Value.Date)
            .OrderBy(g => g.Key)
            .Take(7)
            .Select(g => g.Key.ToString("dd MMM"))
            .ToList();

        ViewBag.ChartData = jobs
            .Where(j => j.CreateDate != null)
            .GroupBy(j => j.CreateDate.Value.Date)
            .OrderBy(g => g.Key)
            .Take(7)
            .Select(g => g.Count())
            .ToList();

        return View(jobs);
    }


    /// Show admin login page
    // ===== LOGIN PAGE =====
    // ===== BUILT-IN ADMIN CREDENTIALS =====
    private const string AdminEmail = "admin@gmail.com";
    private const string AdminPassword = "123";
[HttpGet]
public IActionResult Login()
{
    return View();
}

// ===== LOGIN POST =====
[HttpPost]
public IActionResult Login(LoginViewModel model)
        {
            // ================= ADMIN LOGIN =================
            if (model.Email == "admin@gmail.com" && model.Password == "123")
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("UserName", "Admin");

                TempData["SuccessMessage"] = "👋 Welcome Admin! Login successful.";

                return RedirectToAction("Dashboard", "Admin");
            }
            ViewBag.Error = "Invalid email or password";
            return View(); 
        }

        public IActionResult Applicants()
{
    var applicants = _context.ApplyForms
        .OrderByDescending(a => a.Id)
        .ToList();

    return View(applicants);
}
public IActionResult UpdateStatus(int id, string status)
{
    var app = _context.ApplyForms.FirstOrDefault(a => a.Id == id);
    if (app == null)
        return RedirectToAction("Applicants");

    // status update
    app.Status = status;

    //  notification save
    _context.Notifications.Add(new Notification
    {
        Email = app.Email,
        Message = "Your application is " + status
    });

    //  EMAIL SEND 
    _email.Send(
        app.Email,
        "Application Status Update",
        $"<h3>Your application is <b>{status}</b></h3>"
    );

    //  save all
    _context.SaveChanges();

    TempData["SuccessMessage"] = "Status updated & email sent!";
    return RedirectToAction("Applicants");
}

public IActionResult ApplicantDashboard()
{
    var email = HttpContext.Session.GetString("UserEmail");

    var applications = _context.ApplyForms
        .Where(a => a.Email == email)
        .OrderByDescending(a => a.AppliedDate)
        .ToList();

    return View(applications);
}


}                         