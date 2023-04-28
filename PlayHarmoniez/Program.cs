using Microsoft.EntityFrameworkCore;
using PlayHarmoniez.App_Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Made some modifications in options 
// sqlServerOptionsAction: sqlOptions => { sqlOptions.EnableRetryOnFailure(); }
// doesn't show exception anymore. this allows for multiple retries until connection

builder.Services.AddDbContext<DataContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
 sqlServerOptionsAction: sqlOptions => { sqlOptions.EnableRetryOnFailure(); }));

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
