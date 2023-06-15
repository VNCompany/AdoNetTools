using System.Web;
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

        ViewData["posts"] = _db.Posts.GetUserPosts(user.Id);
        
        return View(user);
    }
    
    [HttpPost]
    public IActionResult Index(string phone, string name, string? email, string? location)
    {
        var user = this.LoginByCookie();
        if (user is null)
            return Redirect("/Home/Index");

        _db.Users.UpdateUserInfo(user.Id, name, phone, email ?? "Не задано", location ?? "Не задано");
        return RedirectToAction("Index");
    }

    public IActionResult NewPost()
    {
        var user = this.LoginByCookie();
        if (user is not null)
            ViewData["User"] = user;
        else
            return Redirect("/Home/Index");

        ViewData["categories"] = _db.Posts.GetCatalog();
        return View();
    }
    
    [HttpPost]
    public IActionResult NewPost(string name, uint catId, string price, string? description, 
        string? location, IFormFileCollection imageFiles)
    {
        var user = this.LoginByCookie();
        if (user is null)
            return Redirect("/Home/Login");

        string formatDescription;
        if (description != null)
            formatDescription = HttpUtility.HtmlEncode(description).Replace("\n", "<br>");
        else
            formatDescription = "Без описания";
        
        uint postId = _db.Posts.Add(user.Id, catId, name, formatDescription, price,
            location ?? user.Location);

        int counter = 1;
        foreach (var file in imageFiles)
        {
            string fileName = $"{postId}_{counter++}.{file.FileName.Split(".")[^1]}";
            using (var fs = new FileStream(Path.Combine(_env.WebRootPath, "content", fileName), FileMode.Create))
            {
                file.CopyToAsync(fs).Wait();
                fs.Flush();
            }
            _db.Posts.AddPicture(postId, fileName);
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult EditPost(uint? id)
    {
        var user = this.LoginByCookie();
        if (user is not null)
            ViewData["User"] = user;
        else
            return Redirect("/Home/Index");

        if (id == null)
            return Redirect("/Profile/Index");

        ViewData["categories"] = _db.Posts.GetCatalog();
        return View(_db.Posts.Get(id.Value));
    }
    
    [HttpPost]
    public IActionResult EditPost(uint? id, string name, uint catId, string price, string? description, string? location)
    {
        var user = this.LoginByCookie();
        if (user is null)
            return Redirect("/Home/Login");
        
        if (id == null)
            return Redirect("/Profile/Index");

        string formatDescription;
        if (description != null)
            formatDescription = HttpUtility.HtmlEncode(description).Replace("\n", "<br>");
        else
            formatDescription = "Без описания";

        _db.Posts.Update(id.Value, catId, name, formatDescription, price, location ?? user.Location);

        return RedirectToAction("Index", "Profile");
    }

    public IActionResult DeletePost(uint? id)
    {
        var user = this.LoginByCookie();
        if (user is null)
            return Redirect("/Home/Login");
        
        if (id == null)
            return Redirect("/Profile/Index");

        var pictures = _db.Posts.GetPictures(id.Value).Select(p => p.Item2);
        foreach (var pictureFileName in pictures)
            System.IO.File.Delete(Path.Combine(_env.WebRootPath, "content", pictureFileName));
        
        _db.Posts.DeletePictures(id.Value);
        _db.Posts.Delete(id.Value);
        
        return RedirectToAction("Index", "Profile");
    }
    
    [HttpPost]
    public IActionResult UpdatePassword(string newPassword)
    {
        var user = this.LoginByCookie();
        if (user is null)
        {
            return RedirectToAction("Index", "Home");
        }
        _db.Users.UpdatePassword(user.Id, newPassword);
        return RedirectToAction("Login");
    }
    
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string phone, string password)
    {
        var user = _db.Users.Get(phone, password);
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
    public IActionResult Register(string name, string phone, string password)
    {
        if (_db.Users.Add(name, phone, password))
            return RedirectToAction("Login");
        ViewData["ErrorMsg"] = "Пользователь с данным номером уже зарегистрирован";
        return View();
    }
}