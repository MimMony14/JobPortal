using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

public class JobsController : Controller
{
    private readonly JobPortalContext _context;

    public JobsController(JobPortalContext context)
    {
        _context = context;
    }

    // Display all jobs
    public IActionResult Index()
    {
        ViewBag.Organizations = new SelectList(
            _context.Organizations.ToList(),
            "OrganizationId",
            "CompanyName"
        );
        var jobs = _context.Jobs
            .Include(j => j.Organization)
            .ToList();

        return View(jobs);
    }

    // Display job details
    public IActionResult Details(int id)
    {
        var job = _context.Jobs.Include(j => j.Organization).FirstOrDefault(j => j.JobId == id);
        if (job == null) 
        {
            return NotFound();
        }

        // Split tags into a list if stored as comma-separated string
        var tags = string.IsNullOrEmpty(job.Tags) ? new List<string>() : job.Tags.Split(',').ToList();

        // Pass tags to ViewBag
        ViewBag.Tags = tags;

        return View(job);
    }
    // POST: handle form submission
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Job job)
    {
        if (ModelState.IsValid)
        {
            job.CreateDate = DateTime.Now;
            _context.Jobs.Add(job);
            _context.SaveChanges();
            return RedirectToAction("Index", "Jobs");
        }
        ViewBag.Organizations = new SelectList(_context.Organizations, "OrganizationId", "CompanyName", job.OrganizationId);
        return View(job);
    }

}
