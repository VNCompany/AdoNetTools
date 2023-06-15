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

        uint? catId = null;
        string? filter = null;
        if (Request.Query.TryGetValue("catId", out var catIdStr)
            && uint.TryParse(catIdStr, out var catIdQuery))
            catId = catIdQuery;
        else if (Request.Query.TryGetValue("q", out var q))
            filter = q; 
        
        return View(_db.Posts.GetAll(catId, filter));
    }

    public IActionResult Catalog()
    {
        var user = this.LoginByCookie();
        if (user is not null)
            ViewData["User"] = user;
        return View(_db.Posts.GetCatalog());
    }
    
    public IActionResult Post(uint? id)
    {
        var user = this.LoginByCookie();
        if (user is not null)
            ViewData["User"] = user;
        
        if (id is null) return StatusCode(409);
        
        var post = _db.Posts.Get(id.Value);
        if (post is null) return StatusCode(404);
        
        return View(post);
    }
}