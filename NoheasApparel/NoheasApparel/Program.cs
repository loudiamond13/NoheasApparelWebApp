using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using NoheasApparel.Utility;
using NoheasApparel.DataAccess;
using NoheasApparel.DataAccess.Repository;
using NoheasApparel.DataAccess.Repository.Interfaces;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//db context
builder.Services.AddDbContext<NoheasApparelContext>
        (options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConString")));







builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<NoheasApparelContext>()
                                                            .AddDefaultTokenProviders();










// lower case the URL
builder.Services.Configure<RouteOptions>(option => option.LowercaseUrls = true);
//stripe configuration
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


////service
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();




////session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    
});
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

StripeConfiguration.ApiKey = builder.Configuration[key:"Stripe:SecretKey"];


app.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
         );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();
app.Run();
