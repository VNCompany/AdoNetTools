using System.Text.Encodings.Web;
using System.Text.Unicode;
using AdsPortal;
using Microsoft.Extensions.WebEncoders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WebEncoderOptions>(options =>
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DbContext>(_ => new DbContext("Host=127.0.0.1;User=root;Password=root;Database=diplom"));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(
    ep => ep.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}"));

app.Run();