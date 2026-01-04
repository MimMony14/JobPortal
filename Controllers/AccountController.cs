using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Linq;

using JobPortal.Models;   
using JobPortal.Data;   

public class AccountController : Controller
{
        private readonly JobPortalContext _context;

        public AccountController(JobPortalContext context)
        {
            _context = context;
        }

        // ================= USER REGISTER =================
[HttpGet]
public IActionResult UserRegister()
{
    return View();
}

[HttpPost]
public IActionResult UserRegister(User model)
{
    if (!ModelState.IsValid)
        return View(model);

    model.Role = "User";
    _context.Users.Add(model);
    _context.SaveChanges();

    TempData["SuccessMessage"] = "🎉 Registration successful! Please login.";
    return RedirectToAction("Login");
}

// ================= ORGANIZATION REGISTER =================
[HttpGet]
public IActionResult OrganizationRegister()
{
    return View();
}

[HttpPost]
public IActionResult OrganizationRegister(Organization model)
{
    if (!ModelState.IsValid)
        return View(model);

    model.Role = "Organization";
    _context.Organizations.Add(model);
    _context.SaveChanges();

    TempData["SuccessMessage"] = "🎉 Organization registered successfully!";
    return RedirectToAction("Login");
}


        // ================= LOGIN =================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // ================= ADMIN LOGIN =================
            if (model.Email == "admin@gmail.com" && model.Password == "admin123")
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("UserName", "Admin");

                TempData["SuccessMessage"] = "👋 Welcome Admin! Login successful.";

                return RedirectToAction("Dashboard", "Admin");
            }

            // ================= USER LOGIN =================
            if (model.Role == "User")
            {
                var user = _context.Users
                    .FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

                if (user == null)
                {
                    ViewBag.Error = "Invalid email or password";
                    return View();
                }

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("Role", "User");

                TempData["SuccessMessage"] = $"👋 Welcome {user.FullName}! Login successful.";

                return RedirectToAction("Applicant", "Home");
            }

            // ================= ORGANIZATION LOGIN =================
            if (model.Role == "Organization")
            {
                var org = _context.Organizations
                    .FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

                if (org == null)
                {
                    ViewBag.Error = "Invalid email or password";
                    return View();
                }

                HttpContext.Session.SetInt32("OrgId", org.OrganizationId);
                HttpContext.Session.SetString("UserName", org.OrganizationName);
                HttpContext.Session.SetString("Role", "Organization");

                TempData["SuccessMessage"] = $"👋 Welcome {org.OrganizationName}! Login successful.";

                return RedirectToAction( "OrgDash",  "Organization", new { id = org.OrganizationId });
            }

            ViewBag.Error = "Invalid login attempt";
            return View();
        }

        // ================= LOGOUT =================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = " You have logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
    }

