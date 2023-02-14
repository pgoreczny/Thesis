using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Thesis.Areas.Identity.Constants;
using Thesis.database;
using Thesis.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CoursesDBContextConnection") ?? throw new InvalidOperationException("Connection string 'CoursesDBContextConnection' not found.");

builder.Services.AddDbContext<CoursesDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CoursesDBContext>();

builder.Services.AddAuthorization(options =>
{
    #region User
    options.AddPolicy(Claims.Users.UserList,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Users.UserList));
    options.AddPolicy(Claims.Users.UserDelete,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Users.UserDelete));
    options.AddPolicy(Claims.Users.UserEdit,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Users.UserEdit));
    options.AddPolicy(Claims.Users.UserAdd,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Users.UserAdd));
    #endregion
    #region Basic
    options.AddPolicy(Claims.Basic.IsRegistered,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Basic.IsRegistered));
    #endregion

    #region Roles
    options.AddPolicy(Claims.Roles.AssignRole,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Roles.AssignRole));
    #endregion
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<MenuService, MenuService>();

builder.Services.AddScoped<UserService, UserService>();

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
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
