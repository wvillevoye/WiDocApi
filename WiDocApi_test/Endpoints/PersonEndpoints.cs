﻿using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WiDocApi_test.Models;

namespace WiDocApi_test.Endpoints
{
    public static class PersonEndpoints
    {
    
      public static void PersonsEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
      {
           
            var group = endpoints.MapGroup("/api")
                        .WithTags("GetPerson");


            if (!string.IsNullOrEmpty(configuration["ApiSettings:ValidApiKey"]))
            {
                group.AddEndpointFilter<WiDocApi_Blazor.WiDocApi.Helpers.ApiKeyAuthFilter>();
            }
            
            
            group.MapGet("/Person/{SearchById}", async (int SearchById, SamplePersonsContext dbContext) =>
            {
                var _person = await dbContext.Persons.SingleOrDefaultAsync(x=>x.PersonID.Equals(SearchById));

                if (_person == null)
                {
                    return Results.NotFound("Person not found.");
                }

                return Results.Ok(_person);
            }).WithName("SearchById").WithOpenApi();
           
            group.MapGet("/Person/search/{SearchStartWithLastName}", async (string SearchStartWithLastName, SamplePersonsContext dbContext) =>
            {
                var _persons = await dbContext.Persons.Where(x=>x.LastName.ToLower().StartsWith(SearchStartWithLastName.ToLower())).ToListAsync();

                if (_persons == null)
                {
                    return Results.NotFound("Persons not found.");
                }

              
                return Results.Ok(_persons);
            }).WithName("SearchStartWithLastName").WithOpenApi();
            
            group.MapPost("/Person", async (Person newPerson, SamplePersonsContext dbContext) =>
            {
                // Add the new person to the database
                dbContext.Persons.Add(newPerson);
                await dbContext.SaveChangesAsync();

                // Return the created person with a 201 status code
                return Results.Created($"/Person/{newPerson.PersonID}", newPerson);
            }).WithName("CreatePerson").WithOpenApi();

            group.MapPut("/Person/{ById}", async (int ById, Person updatedPerson, SamplePersonsContext dbContext) =>
            {
                // Find the person by ID
                var existingPerson = await dbContext.Persons.SingleOrDefaultAsync(x => x.PersonID == ById);

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

                // Save changes to the database
                await dbContext.SaveChangesAsync();

                // Return NoContent to indicate successful update
                return Results.Ok("person is updated successfully.");
            }).WithName("UpdatePerson").WithOpenApi();



            group.MapDelete("/Person/{ById}", async (int ById, SamplePersonsContext dbContext) =>
            {
                // Find the person by ID
                var personToDelete = await dbContext.Persons.SingleOrDefaultAsync(x => x.PersonID == ById);

                if (personToDelete == null)
                {
                    return Results.NotFound("Person not found.");
                }

                // Remove the person from the database
                dbContext.Persons.Remove(personToDelete);
                await dbContext.SaveChangesAsync();

                // Return NoContent to indicate successful deletion
                return Results.NoContent();
            }).WithName("DeletePerson").WithOpenApi();
        }
    }
}
