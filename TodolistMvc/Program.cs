using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodolistMvc.Areas.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System;
using TodolistMvc.Models.Validators;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TodolistMvc") ?? throw new InvalidOperationException("Connection string 'TodolistMvc' not found.");

builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<TaskContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<AuthContext>();

// Validators
builder.Services.AddScoped<IValidator<TodolistMvc.Models.Tasks.TaskNewDTO>, TaskNewValidator>();
builder.Services.AddScoped<IValidator<TodolistMvc.Models.Tasks.TaskEditDTO>, TaskEditValidator>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}");

app.Run();
