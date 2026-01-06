using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobPortal.Data;
using JobPortal.Models;

public class JobsController : Controller
{
    private readonly JobPortalContext _context;

    public JobsController(JobPortalContext context)
    {
        _context = context;
    }

    // Display all jobs with optional keyword and type filters
    public IActionResult Index(string? keyword, List<string>? types)
    {
        ViewBag.Organizations = new SelectList(
            _context.Organizations.ToList(),
            "OrganizationId",
            "OrganizationName"
        );

        var jobs = _context.Jobs
            .Include(j => j.Organization)
            .Include(j => j.Bookmarks)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            jobs = jobs.Where(j =>
                j.Title.Contains(keyword) ||
                j.Address.Contains(keyword) ||
                (j.Organization != null &&
                 j.Organization.OrganizationName.Contains(keyword))
            );
        }

        if (types != null && types.Any())
        {
            jobs = jobs.Where(j => types.Contains(j.JobType));
        }

        return View(jobs.ToList());
    }

    // Display job details
    public IActionResult Details(int id)
    {
        var job = _context.Jobs.Include(j => j.Organization)
                               .FirstOrDefault(j => j.JobId == id);
        if (job == null) return NotFound();

        var tags = string.IsNullOrEmpty(job.Tags)
            ? new List<string>()
            : job.Tags.Split(',').ToList();

        ViewBag.Tags = tags;
        return View(job);
    }

    // GET: Create Job
    public IActionResult Create()
    {
        ViewBag.Organizations = new SelectList(_context.Organizations, "OrganizationId", "OrganizationName");
        return View();
    }

    // POST: Create Job
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

        ViewBag.Organizations = new SelectList(_context.Organizations, "OrganizationId", "OrganizationName", job.OrganizationId);
        return View(job);
    }

    // Toggle bookmark for a job (only for logged-in users)
// Toggle bookmark for a job
[HttpPost]
public IActionResult Toggle([FromBody] int jobId)
{
    int? userId = HttpContext.Session.GetInt32("UserId"); // get logged-in user
    if (userId == null) return Json(new { bookmarked = false, error = "Not logged in" });

    var existing = _context.Bookmarks
        .FirstOrDefault(b => b.UserId == userId.Value && b.JobId == jobId);

    if (existing == null)
    {
        _context.Bookmarks.Add(new Bookmark
        {
            UserId = userId.Value,
            JobId = jobId,
            CreatedAt = DateTime.Now
        });
        _context.SaveChanges();
        return Json(new { bookmarked = true });
    }
    else
    {
        _context.Bookmarks.Remove(existing);
        _context.SaveChanges();
        return Json(new { bookmarked = false });
    }
}

public IActionResult ShowBookmarks()
{
    int? userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null)
        return RedirectToAction("Login", "Account"); // not logged in

    var jobs = _context.Jobs
        .Include(j => j.Organization)
        .Include(j => j.Bookmarks)
        .Where(j => j.Bookmarks.Any(b => b.UserId == userId.Value))
        .ToList();

    return View("~/Views/Applicant/ShowBookmarks.cshtml",jobs);
}
}
