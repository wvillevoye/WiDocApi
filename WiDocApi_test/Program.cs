using Microsoft.EntityFrameworkCore;
using WiDocApi_test.Models;
using WiDocApi_test.Components;
using WiDocApi_Blazor;
using WiDocApi_Blazor.WiDocApi.Helpers;
using WiDocApi_test.Endpoints;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<SamplePersonsContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                     .LogTo(Console.WriteLine, LogLevel.Warning) // Set log level to Warning or higher
                     .EnableSensitiveDataLogging(false)); // Disable sensitive


builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddSiteWiDocApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.PersonsEndpoints(builder.Configuration);

app.Run();
