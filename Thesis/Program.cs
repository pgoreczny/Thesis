using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Thesis.Areas.Identity.Constants;
using Thesis.database;
using Thesis.Models;
using Thesis.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CoursesDBContextConnection") ?? throw new InvalidOperationException("Connection string 'CoursesDBContextConnection' not found.");

builder.Services.AddDbContext<CoursesDBContext>(options =>
    options.UseSqlServer(connectionString)
    );

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
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

    #region CourseManagement
    options.AddPolicy(Claims.ManageCourses.CourseList,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseList));
    options.AddPolicy(Claims.ManageCourses.CourseDelete,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseDelete));
    options.AddPolicy(Claims.ManageCourses.CourseEdit,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseEdit));
    options.AddPolicy(Claims.ManageCourses.CourseAdd,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseAdd));
    options.AddPolicy(Claims.ManageCourses.ManageUsers,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.ManageCourses.ManageUsers));
    #endregion
    #region CourseUserSide
    options.AddPolicy(Claims.UserCourses.AccessCourse,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.UserCourses.AccessCourse));
    options.AddPolicy(Claims.UserCourses.JoinCourse,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.UserCourses.JoinCourse));
    options.AddPolicy(Claims.UserCourses.SeeCourses,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.UserCourses.SeeCourses));
    options.AddPolicy(Claims.UserCourses.ParticipateInCourse,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.UserCourses.ParticipateInCourse));
    #endregion

    #region Forum 
    options.AddPolicy(Claims.Forum.WritePost,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Forum.WritePost)); 
    options.AddPolicy(Claims.Forum.CommentPost,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Forum.CommentPost)); 
    options.AddPolicy(Claims.Forum.EditYour,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Forum.EditYour)); 
    options.AddPolicy(Claims.Forum.EditAny,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Forum.EditAny)); 
    options.AddPolicy(Claims.Forum.Delete,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Forum.Delete));
    options.AddPolicy(Claims.Forum.ReadPost,
        policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.Forum.ReadPost));
    #endregion
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddScoped<MenuService, MenuService>();
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<CourseService, CourseService>();
builder.Services.AddScoped<ActivityService, ActivityService>();
builder.Services.AddScoped<FileService, FileService>();
builder.Services.AddScoped<PaymentService, PaymentService>();
builder.Services.AddScoped<AnswerService, AnswerService>();
builder.Services.AddScoped<ForumService, ForumService>();
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<CalendarService, CalendarService>();

builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    opt.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pl-PL");
    opt.DefaultRequestCulture.Culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
    opt.DefaultRequestCulture.Culture.DateTimeFormat.DateSeparator = ".";
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
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
