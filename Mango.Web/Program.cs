using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IShoppingCartService, ShoppingCartService>();

StaticDetails.CouponAPIBase = builder.Configuration.GetSection("ServiceUrls:CouponAPI").Get<string>();
StaticDetails.AuthAPIBase = builder.Configuration.GetSection("ServiceUrls:AuthAPI").Get<string>();
StaticDetails.ProductAPIBase = builder.Configuration.GetSection("ServiceUrls:ProductAPI").Get<string>();
StaticDetails.ShoppingCartAPIBase = builder.Configuration.GetSection("ServiceUrls:ShoppingCartAPI").Get<string>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.ExpireTimeSpan = TimeSpan.FromHours(10);
        option.ReturnUrlParameter = "";
        option.Events = new CookieAuthenticationEvents()
        {
            OnRedirectToAccessDenied = (context) =>
            {
                return Invoke404Response(context);
            },
            OnRedirectToLogin = (context) =>
            {
                return Invoke404Response(context);
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

Task Invoke404Response(RedirectContext<CookieAuthenticationOptions> context)
{
    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
    return context.Response.CompleteAsync();
}