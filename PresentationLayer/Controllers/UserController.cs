using System.Security.Claims;
using BusinessLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

public class UserController(UserService userService) : AppController
{
    public IActionResult Register() => View(new UserViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(UserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await userService.RegisterAsync(model.Username.Trim(), model.Password, model.Email.Trim());
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(model);
        }

        Flash("Your account was created. Please sign in.");
        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login(string? returnUrl = null) => View(new UserLoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await userService.AuthenticateAsync(model.UserName.Trim(), model.Password);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Login failed. Check your user name and password.");
            return View(model);
        }

        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
        ], CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = model.RememberMe });

        return Url.IsLocalUrl(model.ReturnUrl) ? Redirect(model.ReturnUrl) : RedirectToAction("Dashboard", "Home");
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied() => View();
}
