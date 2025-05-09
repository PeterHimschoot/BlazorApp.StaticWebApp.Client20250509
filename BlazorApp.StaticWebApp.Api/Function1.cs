using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BlazorApp.StaticWebApp.Api
{
  [DebuggerDisplay("Forecasts {Date} - {TemperatureC}")]
  public class WeatherForecast
  {
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
  }

  public class Function1
  {
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
      _logger = logger;
    }

    [Function("WeatherForecast")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {

      // Pretend this talks to a database
      var startDate = DateOnly.FromDateTime(DateTime.Now);
      var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
      ValueTask<WeatherForecast[]> result = ValueTask.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = startDate.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = summaries[Random.Shared.Next(summaries.Length)]
      }).ToArray());
      return new OkObjectResult(result);
    }
  }
}
