﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using WiDocApi_Blazor.WiDocApi.Helpers;
using WiDocApi_Blazor.WiDocApi.Models;
using WiDocApi_test.Models;



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
            //********
            group.MapGet("/Test/{_String}/{_bool:bool}/{_int:int}/{_enum:SampleEnum}/{_enum1:ProgramLangEnum}/{_date:datetime}/",
              (string _String, bool _bool, int _int, SampleEnum _enum, ProgramLangEnum _enum1, DateTime _date) =>
              {
                  var _res = JsonSerializer.Serialize(new Dictionary<string, object>
                    {
                        {"String", _String},
                        {"bool", _bool.ToString()},
                        {"int", _int.ToString()},
                        {"date", _date.ToString("yyyy-MM-dd HH:mm:ss")}, // Ensuring proper date format
                        {"enum", _enum.ToString()},
                        {"enum1", _enum1.ToString()}
                    }, new JsonSerializerOptions { WriteIndented = true });

                  return Results.Json(_res);
              })
           .WithName("Test123")
           .WithOpenApi()
           .AddWiDocApiEndpoints(new EndpointInfo
           {
               Group = "Test",
               Description = "Test with string, int, bool , 2 enum and datime",
               CacheDurationMinutes = 0,
               EnumLists = EnumUtils.CreateEnumLists(
                            ("_enum", typeof(SampleEnum)),
                            ("_enum1", typeof(ProgramLangEnum))
                           )
           });
            //*********

            group.MapGet("/Person/{SearchById:int}", async (int SearchById, SamplePersonsContext dbContext) =>
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

            group.MapGet("/Person/search/{SearchStartWithLastName}", async (string SearchStartWithLastName, SamplePersonsContext dbContext) =>
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
                //Active = false,
            });

            group.MapPost("/Person", async (Person newPerson, SamplePersonsContext dbContext) =>
            {
                // Add the new person to the database
                dbContext.Persons.Add(newPerson);
                await dbContext.SaveChangesAsync();

                // Return the created person with a 201 status code
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

            group.MapPut("/Person/{ById:int}", async (int ById, Person updatedPerson, SamplePersonsContext dbContext) =>
            {
                // Find the person by ID
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

                // Save changes to the database
                await dbContext.SaveChangesAsync();

                // Return NoContent to indicate successful update
                return Results.Ok("person is updated successfully.");
            })
            .WithName("UpdatePerson")
            .WithOpenApi()
            .AddWiDocApiEndpoints(new EndpointInfo
            {
                Group = "RestPerson",
                Description = "Update a person by ID",
            });

            group.MapDelete("/Person/{ById:int}", async (int ById, SamplePersonsContext dbContext) =>
            {
                // Find the person by ID
                var personToDelete = await dbContext.Persons.FindAsync(ById); ;

                if (personToDelete == null)
                {
                    return Results.NotFound("Person not found.");
                }

                // Remove the person from the database
                dbContext.Persons.Remove(personToDelete);
                await dbContext.SaveChangesAsync();

                // Return NoContent to indicate successful deletion
                return Results.NoContent();
            })
             .WithName("DeletePerson")
             .WithOpenApi()
             .AddWiDocApiEndpoints(new EndpointInfo
              {
                  Group = "RestPerson",
                  Description = "Delete a person by ID"
              });



        }
        public enum SampleEnum
        {
            Option1,
            Option2,
            Option3
        }

        public enum ProgramLangEnum
        {
            cobel,
            python,
            csharp,
        }
    }
}
