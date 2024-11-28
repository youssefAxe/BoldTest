
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var app = builder.Build();
// Add services to the container.
string appSettingsPath = Path.Combine(AppContext.BaseDirectory, "Settings", "appsettings.json");
// Add the JSON file to configuration
builder.Configuration.AddJsonFile(appSettingsPath, optional: true, reloadOnChange: true);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.UseAuthorization();
app.MapRazorPages();
app.Run();