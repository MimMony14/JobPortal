using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobPortal.Data;     // ⭐ DbContext
using JobPortal.Models;   // ⭐ Models
public class AdminController : Controller
{
    private readonly JobPortalContext _context;
    public AdminController(JobPortalContext context)
    {
        _context = context;
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
                Company = j.Organization.OrganizationName
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
}