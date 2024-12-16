//Entry point for application execution



using Blogspot.Data;
using Blogspot.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//DB context for Blogs and Tags
builder.Services.AddDbContext<BlogspotDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("BlogDbConnectionString")));

//DB context for Authentication & Authorization
builder.Services.AddDbContext<AuthDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();


builder.Services.Configure<IdentityOptions>(options =>
{
    //Default settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddScoped<ITagRepository,TagRepository>(); //For Tag
builder.Services.AddScoped<IBlogPostRepository,BlogPostRepository>(); //For BlogPost
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();//For image upload to cloudinary
builder.Services.AddScoped<IBlogPostLikeRepository,BlogPostLikeRepository>();//For likes
builder.Services.AddTransient<IBlogPostCommentRepository, BlogPostCommentRepository>();//For comments
builder.Services.AddScoped<IUserRepository,UserRepository>(); //For all users list
builder.Services.AddTransient<IEmailSender, EmailSender>();


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

//Default page i.e. Home 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
