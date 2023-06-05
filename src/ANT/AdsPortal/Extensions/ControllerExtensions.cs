using Microsoft.AspNetCore.Mvc;

using AdsPortal.Models;

namespace AdsPortal.Extensions;

public static class ControllerExtensions
{
    public static User? LoginByCookie(this Controller controller)
    {
        if (controller.Request.Cookies.TryGetValue("phone", out var phone)
            && controller.Request.Cookies.TryGetValue("password", out var password))
        {
            var context = controller.HttpContext.RequestServices.GetRequiredService<DbContext>();
            return context.Login(phone!, password!);
        }

        return null;
    }
}