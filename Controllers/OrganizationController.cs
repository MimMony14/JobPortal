using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobPortal.Services;
using JobPortal.Data;     
using JobPortal.Models;   
public class OrganizationController : Controller
{
    private readonly JobPortalContext _context;
    private readonly EmailService _email; // CHANGED: inject email service if you want notifications

    public OrganizationController(JobPortalContext context, EmailService email) // CHANGED: added email service
    {
        _context = context;
        _email = email;
    }

    public IActionResult OrgDash(int id)
    {
        var jobs = _context.Jobs
            .Include(j => j.Organization)
            .Include(j => j.ApplyForms) // navigation property
            .Where(j => j.OrganizationId == id)
            .ToList();
            
        ViewBag.OrganizationId = id;
        return View(jobs); 
    }

    // CHANGED: Add UpdateStatus for org
    public IActionResult UpdateStatus(int id, string status, int orgId)
{
    var app = _context.ApplyForms.FirstOrDefault(a => a.Id == id);
    if (app == null)
        return RedirectToAction("OrgDash", new { id = orgId });

    // update status
    app.Status = status;

    // send email only
    _email.Send(
        app.Email,
        "Application Status Update",
        $"<h3>Your application is <b>{status}</b></h3>"
    );

    // save changes
    _context.SaveChanges();

    TempData["SuccessMessage"] = "Status updated & email sent!";
    return RedirectToAction("OrgDash", new { id = orgId });
}

}