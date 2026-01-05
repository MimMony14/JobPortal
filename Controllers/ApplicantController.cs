using Microsoft.AspNetCore.Mvc;
using JobPortal.Data;
using JobPortal.Models;
using System.Linq;

namespace JobPortal.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly JobPortalContext _context;

        public ApplicantController(JobPortalContext context)
        {
            _context = context;
        }

        // ===== APPLICANT DASHBOARD =====
        public IActionResult Dashboard()
        {
           
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var apps = _context.ApplyForms
                .Where(a => a.Email == email)
                .OrderByDescending(a => a.AppliedDate)
                .ToList();

            // stats for ViewBag
            ViewBag.Total = apps.Count;
            ViewBag.Pending = apps.Count(a => a.Status == "Pending");
            ViewBag.Accepted = apps.Count(a => a.Status == "Accepted");
            ViewBag.Rejected = apps.Count(a => a.Status == "Rejected");

            return View("~/Views/Applicant/Dashboard.cshtml", apps); // Dashboard.cshtml
        }
    }
}
