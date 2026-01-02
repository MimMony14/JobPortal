using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
public class OrganizationController : Controller
{
    private readonly JobPortalContext _context;
    public OrganizationController(JobPortalContext context)
    {
        _context = context;
    }
    public IActionResult OrgDash(int id)
    {
        var jobs = _context.Jobs
        .Include(j => j.Organization)
        .Where(j => j.OrganizationId == id)
        .ToList();
        return View(jobs);
    }
}