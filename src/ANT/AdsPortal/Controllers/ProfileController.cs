using Microsoft.AspNetCore.Mvc;

using AdsPortal.Extensions;

namespace AdsPortal.Controllers;

public class ProfileController : Controller
{
    private readonly DbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProfileController(DbContext dbContext, IWebHostEnvironment hostEnvironment)
    {
        _db = dbContext;
        _env = hostEnvironment;
    }

    public IActionResult Index()
    {
        var user = this.LoginByCookie();
        if (user is not null)
            ViewData["User"] = user;
        else
            return Redirect("/Home/Index");
        return View(user);
    }
    
    [HttpPost]
    public IActionResult Index(string? phone, string? email, string? location)
    {
        var user = this.LoginByCookie();
        if (user is null)
            return Redirect("/Home/Index");

        _db.UpdateUserInfo(user.Id, phone ?? String.Empty, email ?? String.Empty, location ?? String.Empty);
        return RedirectToAction("Index");
    }

    public IActionResult NewPost()
    {
        var user = this.LoginByCookie();
        if (user is not null)
            ViewData["User"] = user;
        else
            return Redirect("/Home/Index");
        return View(_db.GetCatalog());
    }
    
    [HttpPost]
    public IActionResult NewPost(IFormFile picture, string name, uint catId, string price, string? description, 
        string? location)
    {
        var user = this.LoginByCookie();
        if (user is null)
            return Redirect("/Home/Login");

        using (var fs = new FileStream(Path.Combine(_env.WebRootPath, "content", picture.FileName), FileMode.Create))
        {
            picture.CopyToAsync(fs);
            fs.Flush();
        }
        
        _db.AddPost(user.Id, catId, name, description ?? "", price, location ?? "", picture.FileName);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult UpdatePassword(string newPassword)
    {
        var user = this.LoginByCookie();
        if (user is null)
        {
            return RedirectToAction("Index", "Home");
        }
        _db.UpdatePassword(user.Id, newPassword);
        return RedirectToAction("Login");
    }
    
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string phone, string password)
    {
        var user = _db.Login(phone, password);
        if (user is null)
        {
            ViewData["ErrorMsg"] = "Неправильный логин или пароль";
            return View();
        }

        Response.Cookies.Append("phone", phone,
            new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(4), Path = "/" });
        Response.Cookies.Append("password", password,
            new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(4), Path = "/" });

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(string phone, string password)
    {
        if (_db.Register(phone, password))
            return RedirectToAction("Login");
        ViewData["ErrorMsg"] = "Пользователь с данным номером уже зарегистрирован";
        return View();
    }
}