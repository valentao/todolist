using Microsoft.EntityFrameworkCore;
using Todolist.Data;
using Microsoft.AspNetCore.Identity;
using Todolist.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages(options =>
{
    options.RootDirectory = "/Pages/Tasks";
    options.Conventions.AuthorizeFolder("/Tasks/");
    options.Conventions.AuthorizePage("/Index");
});

builder.Services.AddDbContext<TodolistContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Todolist")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Todolist")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<AuthDbContext>();

// registration options configurations
builder.Services.Configure<IdentityOptions>(options =>
    options.Password.RequireUppercase = false
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
