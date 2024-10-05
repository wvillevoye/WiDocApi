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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<SamplePersonsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging());

builder.Services.AddScoped<IPersonService, PersonService>();

//builder.Services.AddRouting(options =>
//{
//    //options.ConstraintMap.Add("sampleEnum", typeof(EnumRouteConstraint<SampleEnum>));
//    //options.ConstraintMap.Add("ProgramLangEnum", typeof(EnumRouteConstraint<ProgramLangEnum>));
//    options.ConstraintMap.Add("select", typeof(SelectConstraint));


//});
builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddSiteWiDocApi();

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

using var scope = app.Services.CreateScope();
var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();
var statelist = await personService.GetStateAsync();
var Selectstatelist = WiDocApi_Blazor.WiDocApi.Helpers.EnumUtils.ListToDictionary(statelist);



app.PersonsEndpoints(builder.Configuration, Selectstatelist);


app.Run();

