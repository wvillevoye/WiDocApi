using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiDocApi_Blazor.WiDocApi.Models;
using WiDocApi_test.Models;
using Microsoft.AspNetCore.Http;
using WiDocApi_Blazor.WiDocApi.Helpers;

namespace WiDocApi_test.Endpoints
{
    public static class PersonEndpoints
    {
        public static void PersonsEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
        {
            var baseurlApi = "api/";
            var group = endpoints.MapGroup(baseurlApi)
                                  .WithTags("Persons");

            if (!string.IsNullOrEmpty(configuration["ApiSettings:ValidApiKey"]))
            {
                group.AddEndpointFilter<WiDocApi_Blazor.WiDocApi.Helpers.ApiKeyAuthFilter>();
            }

            group.MapGet("/Person/{SearchById}", async (int SearchById, SamplePersonsContext dbContext) =>
            {
                var _person = await dbContext.Persons.FindAsync(SearchById);

                if (_person == null)
                {
                    return Results.NotFound("Person not found.");
                }
                return Results.Ok(_person);
            })
            .WithName("SearchById")
            .WithOpenApi()
            .AddWiDocApiEndpoints(new EndpointInfo
            {
                Group = "GetPerson",
                Description = "Search person by ID",
                CacheDurationMinutes = 10,
            });

            group.MapGet("/Person/search/{SearchStartWithLastName}/{city}", async (string SearchStartWithLastName, string city, SamplePersonsContext dbContext) =>
            {
                var _persons = await dbContext.Persons.Where(x => x.LastName.ToLower().StartsWith(SearchStartWithLastName.ToLower())).ToListAsync();

                if (_persons == null)
                {
                    return Results.NotFound("Persons not found.");
                }
                return Results.Ok(_persons);
            })
            .WithName("SearchStartWithLastName")
            .WithOpenApi()
            .AddWiDocApiEndpoints(new EndpointInfo
            {
                Group = "GetPerson",
                Description = "Search person by last name starting with",
                CacheDurationMinutes = 10,
            });

            group.MapPost("/Person", async (Person newPerson, SamplePersonsContext dbContext) =>
            {
                dbContext.Persons.Add(newPerson);
                await dbContext.SaveChangesAsync();

                return Results.Created($"/Person/{newPerson.PersonID}", newPerson);
            })
            .WithName("CreatePerson")
            .WithOpenApi()
            .AddWiDocApiEndpoints(new EndpointInfo
            {
                Group = "RestPerson",
                Description = "Create a new person",
                RequiresInput = false,
            });

            group.MapPut("/Person/{ById}", async (int ById, Person updatedPerson, SamplePersonsContext dbContext) =>
            {
                var existingPerson = await dbContext.Persons.FindAsync(ById);

                if (existingPerson == null)
                {
                    return Results.NotFound("Person not found.");
                }

                // Update only the fields that are provided
                if (!string.IsNullOrEmpty(updatedPerson.FirstName))
                    existingPerson.FirstName = updatedPerson.FirstName;

                if (!string.IsNullOrEmpty(updatedPerson.LastName))
                    existingPerson.LastName = updatedPerson.LastName;

                if (updatedPerson.Age.HasValue)
                    existingPerson.Age = updatedPerson.Age;

                if (!string.IsNullOrEmpty(updatedPerson.Gender))
                    existingPerson.Gender = updatedPerson.Gender;

                if (!string.IsNullOrEmpty(updatedPerson.Email))
                    existingPerson.Email = updatedPerson.Email;

                if (!string.IsNullOrEmpty(updatedPerson.State))
                    existingPerson.State = updatedPerson.State;

                if (!string.IsNullOrEmpty(updatedPerson.ZipCode))
                    existingPerson.ZipCode = updatedPerson.ZipCode;

                await dbContext.SaveChangesAsync();

                return Results.Ok("Person is updated successfully.");
            })
            .WithName("UpdatePerson")
            .WithOpenApi()
            .AddWiDocApiEndpoints(new EndpointInfo
            {
                Group = "RestPerson",
                Description = "Update a person by ID",
            });

            group.MapDelete("/Person/{ById}", async (int ById, SamplePersonsContext dbContext) =>
            {
                var personToDelete = await dbContext.Persons.FindAsync(ById);

                if (personToDelete == null)
                {
                    return Results.NotFound("Person not found.");
                }

                dbContext.Persons.Remove(personToDelete);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            })
            .WithName("DeletePerson")
            .WithOpenApi()
            .AddWiDocApiEndpoints(new EndpointInfo
            {
                Group = "RestPerson",
                Description = "Delete a person by ID",
            });

            // Now, inspect the parameters from the endpoint metadata
       
        }

    

    }
}
