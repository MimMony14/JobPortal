using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using JobPortal.Data;     //  DbContext
using JobPortal.Models;   //  Job model




public class JobsController : Controller
{
    private readonly JobPortalContext _context;

    public JobsController(JobPortalContext context)
    {
        _context = context;
    }
     
    // Display all jobs
    //public IActionResult Index()
   // {
       // ViewBag.Organizations = new SelectList(
           /// _context.Organizations.ToList(),
           /// "OrganizationId",
           /// "OrganizationName"
      //  );
       // var jobs = _context.Jobs
          //  .Include(j => j.Organization)
          //  .ToList();

       // return View(jobs);
   // }
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
    public IActionResult Create()
    {
        ViewBag.Organizations = new SelectList(_context.Organizations, "OrganizationId", "OrganizationName");
        return View();
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
        ViewBag.Organizations = new SelectList(_context.Organizations, "OrganizationId", "OrganizationName", job.OrganizationId);
        return View(job);
    }
    [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult BookmarkToggle([FromBody] BookmarkToggleModel model)
{
    var userIdStr = HttpContext.Session.GetString("UserId");
    if (!int.TryParse(userIdStr, out int userId))
        return Json(new { success = false });

    var bookmark = _context.Bookmarks
        .FirstOrDefault(b => b.JobId == model.JobId && b.UserId == userId);

    if (bookmark == null)
    {
        // Add bookmark
        _context.Bookmarks.Add(new Bookmark { JobId = model.JobId, UserId = userId });
    }
    else
    {
        // Remove bookmark
        _context.Bookmarks.Remove(bookmark);
    }

    _context.SaveChanges();
    return Json(new { success = true });
}

// DTO for AJAX
public class BookmarkToggleModel
{
    public int JobId { get; set; }
}

}
