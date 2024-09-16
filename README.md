# WiDocApi
### This version now includes specific examples of both `apisettings.json` and `appsettings.json` to give users clear instructions on how to configure and use the API settings.
### This updated version includes the mention of the **WiDocApi_test** project, which provides an example of how to integrate the WiDocApi library into a Blazor app.
**WiDocApi** is a Blazor .NET 8 program built as a Razor Class Library that provides a Swagger-like interface for API documentation and interaction. The project supports API key integration and manages API calls through a JSON configuration file in the `Endpoints` directory of the main Blazor application.

## Features

- **Swagger-like Interface**: WiDocApi offers an easy-to-use interface for exploring and testing API endpoints.
- **API Key Integration**: API calls are secured using an API key stored in the `appsettings.json` file.
- **Single Razor Component Page**: The interface is rendered as a single Razor component, making it easy to integrate.
- **JSON-Based API Configuration**: API calls are defined in an `apisettings.json` file, enabling easy management of endpoints.

## Example of `apisettings.json`

Below is an example of how the API endpoints are configured in the `apisettings.json` file. Each entry includes information about the endpoint, such as its method, path, description, and caching behavior:


![Afbeelding](WiDocApi.png)


```json
[
  {
    "Id": 1,
    "BaseUrl": null,
    "Group": "Group1",
    "Path": "/api/Person/{SearchById}",
    "Description": "Returns information about the person specified by Id.",
    "Method": "GET",
    "RequiresInput": true,
    "CacheDurationMinutes": 10
  },
  {
    "Id": 2,
    "BaseUrl": null,
    "Group": "Group1",
    "Path": "/api/Person/search/{SearchStartWithLastName}",
    "Description": "Returns a list of persons starting with the specified last name.",
    "Method": "GET",
    "RequiresInput": true,
    "CacheDurationMinutes": 10
  },
  {
    "Id": 3,
    "BaseUrl": null,
    "Group": "Group2",
    "Path": "/api/Person",
    "Description": "Adds a new person.",
    "Method": "POST",
    "RequiresInput": false,
    "CacheDurationMinutes": 0
  },
  {
    "Id": 4,
    "BaseUrl": null,
    "Group": "Group2",
    "Path": "/api/Person/{ById}",
    "Description": "Updates a person's information.",
    "Method": "PUT",
    "RequiresInput": true
  },
  {
    "Id": 5,
    "BaseUrl": null,
    "Group": "Group2",
    "Path": "/api/Person/{ById}",
    "Description": "Deletes a person.",
    "Method": "DELETE",
    "RequiresInput": true
  }
]
```
## The appsettings.json file
```json
{
  "ApiSettings": {
    "ValidApiKey": "test"
  }
}
```
