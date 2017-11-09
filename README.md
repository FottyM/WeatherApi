# Readme
*important* => the main branch is `master2` not `master`.

## Setup and execution

1. Before anything make sure you have .NET as explained on from this link
[Get started with .NET in 10 minutes](https://www.microsoft.com/net/learn/get-started/macos) .

2. Once that is done, please clone this repository: `git clone https://github.com/FottyM/WeatherApi.git`.

3. The project is structured like so: 
```
├── WeatherApi2
│   ├── Controllers
│   ├── Program.cs
│   ├── Properties
│   ├── README.md
│   ├── Startup.cs
│   ├── WeatherApi2.csproj
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── bin
│   ├── obj
│   └── wwwroot
├── WeatherApi2.sln
├── WeatherApi2.userprefs
└── WeatherApi2Test
    ├── WeatherApi2Test.csproj
    ├── WeatherControllerIntegrationTests.cs
    ├── WeatherControllerUnitTests.cs
    ├── bin
    └── obj
```
The two most important folders are *WeatherApi2* and *WeatherApi2Test*, the former contains the project and the later conter the test, just like their names suggest.

4. from the root folder execute the command `dotnet build` to build the project.
5. execute the command `cd WeatherApi2` to enter *WeatherApi2* then `dotnet run`, this command will compile and run the application.
6. In the terminal this will show.
```
Hosting environment: Development
Content root path: ../WeatherApi2/WeatherApi2
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.

```
7. Visit `http://localhost:5000/api/weather` and you will get this `{ "message": "now try http://localhost:5000/api/weather?lat=47.641944&lon=-122.127222"}`
8. To get the weather forecast of a particular place on earth in Kelvin please specify it in the url like so `http://localhost:5000/api/weather?lat={value}&lon={value}`
Exampe: `http://localhost:5000/api/weather?lon=26.7321547&lat=58.38288670` will return :
```
{
    "temperatureOne": 40.5,
    "temperatureTwo": 41,
    "average": 40.75,
    "timezone": "Europe/Tallinn",
    "name": "Tartu"
}
```
## Testing

For testing I used [xUnit](https://xunit.github.io/docs/getting-started-dotnet-core) with [Fluentassertion](http://fluentassertions.com/documentation.html) to write the integration test. And to test to run the test:

1. `cd WeatherApi2Test`
2. `dotnet xunit` or `dotnet test` should do the same job.


## Contacts
For more question pleas contact me via email.