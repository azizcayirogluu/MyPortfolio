using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class LoginController : Controller
{
  [HttpGet]
  public IActionResult Index() => View();

  [HttpPost]
  public async Task<IActionResult> Index(LoginViewModel model)
  {
    // Şimdilik basit kontrol
    if (model.Username == "azizca" && model.Password == "105566")
    {
      var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Username) };
      var userIdentity = new ClaimsIdentity(claims, "a");
      ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

      await HttpContext.SignInAsync(principal);
      return RedirectToAction("Index", "Statistic");
    }

    ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
    return View();
  }

  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Index", "Login");
  }
}