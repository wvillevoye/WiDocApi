using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Text.Json;
using WiDocApi_Blazor.WiDocApi.Helpers;
using WiDocApi_Blazor.WiDocApi.Models;
using WiDocApi_test.Models;
using WiDocApi_test.Services;


namespace WiDocApi_test.Endpoints
{
    public static class PersonEndpoints
    {
        public static void PersonsEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration configuration, Dictionary<string,string> states)
        {
            var baseurlApi = "api/";
            var group = endpoints.MapGroup(baseurlApi)
                .WithTags("Persons");

            if (!string.IsNullOrEmpty(configuration["ApiSettings:ValidApiKey"]))
            {
                group.AddEndpointFilter<WiDocApi_Blazor.WiDocApi.Helpers.ApiKeyAuthFilter>();
            }
           
            //********
            group.MapGet("/Person/Test/{SampleString}/{SampleBool:bool}/{SampleInt:int}/{SampleList:select}/{SampleList1:select}/{SampleDate:datetime}/{StatesList:select}",
                (string SampleString, bool SampleBool, int SampleInt, SampleEnum SampleList, ProgramLangEnum SampleList1, DateTime SampleDate, string StatesList) =>
                {
                    var _res = new Dictionary<string, object>
                    {
                            {"String", SampleString},
                            {"bool", SampleBool.ToString()},
                            {"int", SampleInt.ToString()},
                            {"date", SampleDate.ToString("yyyy-MM-dd HH:mm:ss")}, // Ensuring proper date format
                            {"enum", SampleList.ToString()},
                            {"enum1", SampleList1.ToString()},
                            {"state", StatesList.ToString()}
                    };

                    return Results.Json(_res, new JsonSerializerOptions { WriteIndented = true });
                })
                .WithName("Test123")
                .WithOpenApi()
                .AddWiDocApiEndpoints(new EndpointInfo
                {
                    Group = "Test",
                    Description = "Test with string, int, bool, 2 enum, and datetime",
                    CacheDurationMinutes = 0,
                    SelectLists = WiDoApiUtils.CreateSelectInput("SampleList",WiDoApiUtils.SelectValueType.Text, WiDoApiUtils.EnumToDictionary<SampleEnum>()["SampleEnum"])
                                         .AddWithChain("SampleList1", WiDoApiUtils.SelectValueType.Text, WiDoApiUtils.EnumToDictionary<ProgramLangEnum>()["ProgramLangEnum"])
                                         .AddWithChain("StatesList", WiDoApiUtils.SelectValueType.Text, states)
                });
            //*********

            group.MapGet("/Person/{SearchById:int}", async (int SearchById, IPersonService personService) =>
            {
                var _person = await personService.GetPersonByIdAsync(SearchById);

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


            group.MapGet("/Person/search/{SearchStartWithLastName}/{State:select}",
                async (string SearchStartWithLastName, string State, IPersonService personService) =>
            {
                // Use the injected personService here

                var _persons = await personService.GetPersonsByLastNameAsync(SearchStartWithLastName, State);


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
                SelectLists = WiDoApiUtils.CreateSelectInput("State", WiDoApiUtils.SelectValueType.Text, states)

            });



            group.MapGet("/States", async (IPersonService personService) =>
        {
                var _state = await personService.GetStateAsync();
                if (_state == null)
                {
                    return Results.NotFound("State list is empty");
                }
                return Results.Ok(_state);
         })
          .WithName("States")
          .WithOpenApi()
          .AddWiDocApiEndpoints(new EndpointInfo
          {
              Group = "GetStates",
              Description = "All States",
              //CacheDurationMinutes = 10,
          });

        group.MapPost("/Person", async (Person newPerson, IPersonService personService) =>
        {
                var createdPerson = await personService.AddPersonAsync(newPerson);
                return Results.Created($"/Person/{createdPerson.LastName}", createdPerson);
        })
        .WithName("CreatePerson")
        .WithOpenApi()
        .AddWiDocApiEndpoints(new EndpointInfo
         {
              Group = "RestPerson",
              Description = "Create a new person",
              RequiresInput = false,
         });

        group.MapPut("/Person/{ById:int}", async (int ById, Person updatedPerson, IPersonService personService) =>
            {
                var result = await personService.UpdatePersonAsync(ById, updatedPerson);
                if (result == null)
                {
                    return Results.NotFound("Person not found.");
                }
                return Results.Ok(result);
        })
         .WithName("UpdatePerson")
         .WithOpenApi()
         .AddWiDocApiEndpoints(new EndpointInfo
          {
             Group = "RestPerson",
             Description = "Update a person by ID",
           });

        group.MapDelete("/Person/{ById:int}", async (int ById, IPersonService personService) =>
        {
                var deleted = await personService.DeletePersonAsync(ById);
                if (!deleted)
                {
                    return Results.NotFound("Person not found.");
                }
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
            cobol,
            python,
            csharp
        }
    }
}
