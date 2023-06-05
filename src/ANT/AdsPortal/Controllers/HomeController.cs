using Microsoft.AspNetCore.Mvc;

using AdsPortal.Extensions;

namespace AdsPortal.Controllers;

public class HomeController : Controller
{
    private readonly DbContext _db;

    public HomeController(DbContext dbContext)
    {
        _db = dbContext;
    }

    public IActionResult Index()
    {
        var user = this.LoginByCookie();
        if (user is not null)
            ViewData["User"] = user;
        return View(_db.GetPosts());
    }

    public IActionResult Catalog()
    {
        var user = this.LoginByCookie();
        if (user is not null)
            ViewData["User"] = user;
        return View(_db.GetCatalog());
    }
    
    public IActionResult Post(uint? id)
    {
        var user = this.LoginByCookie();
        if (user is not null)
            ViewData["User"] = user;
        
        if (id is null) return StatusCode(409);
        
        var post = _db.GetPost(id.Value);
        if (post is null) return StatusCode(404);
        
        ViewData["PostUser"] = _db.GetUserById(post.UserId);
        return View(post);
    }
}