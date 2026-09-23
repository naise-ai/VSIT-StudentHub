using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
namespace StudentManagementSystem.Controllers;
public class AccountController : Controller
{
    [HttpGet] public IActionResult Login(string? returnUrl = null) { ViewBag.ReturnUrl = returnUrl; return View(new LoginViewModel()); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);
        if (model.Email.Equals("naise.shekhar@vsit.edu.in", StringComparison.OrdinalIgnoreCase) && model.Password == "Admin@123")
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "VSIT Administrator"), new Claim(ClaimTypes.Email, model.Email), new Claim(ClaimTypes.Role, "Admin") };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)), new AuthenticationProperties { IsPersistent = model.RememberMe });
            return Redirect(returnUrl ?? "/");
        }
        ModelState.AddModelError("", "Invalid email or password."); return View(model);
    }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Logout() { await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); return RedirectToAction(nameof(Login)); }
}
