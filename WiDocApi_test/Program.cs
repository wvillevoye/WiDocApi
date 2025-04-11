// Using directives at the top of your Program.cs
using Microsoft.EntityFrameworkCore;
using WiDocApi_test.Models;
using WiDocApi_test.Components;
using WiDocApi_Blazor;
using WiDocApi_Blazor.WiDocApi.Helpers;
using WiDocApi_test.Endpoints;
using Microsoft.Extensions.Configuration;
using static WiDocApi_test.Endpoints.PersonEndpoints;
using WiDocApi_test.Services;
using System.Collections.Specialized;
using WiDocApi_Blazor.WiDocApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<SamplePersonsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging());

builder.Services.AddScoped<IPersonService, PersonService>();


builder.Services.AddQuickGridEntityFrameworkAdapter();

//builder.Services.AddSiteWiDocApi();

builder.Services.AddSiteWiDocApi(new WiDocApiApikeySettings()
{
    ApiKeys = builder.Configuration.GetSection("WiDocApiKeySetting:ApiKey").Get<List<string>>()!,
    ApiKeyHeaderName = builder.Configuration.GetSection("WiDocApiKeySetting:ApiKeyHeaderName").Value
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Create a scope for services, initialize state list, and set up endpoint mappings
using (var scope = app.Services.CreateScope())
{
    var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();
    var statelist = await personService.GetStateAsync();
    var selectStateList = WiDoApiUtils.ListToDictionary(statelist);

    // Add Persons endpoints and pass state list
    app.PersonsEndpoints(builder.Configuration, selectStateList);
}

app.Run();

