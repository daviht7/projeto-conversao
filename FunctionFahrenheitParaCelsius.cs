using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ConversaoTemperatura
{
  public class FunctionFahrenheitParaCelsius
  {
    private readonly ILogger<FunctionFahrenheitParaCelsius> _logger;

    public FunctionFahrenheitParaCelsius(ILogger<FunctionFahrenheitParaCelsius> log)
    {
      _logger = log;
    }

    [FunctionName("ConverterFahrenheitParaCelsius")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Conversao" })]
    [OpenApiParameter(name: "fahrenheit", In = ParameterLocation.Path, Required = true, Type = typeof(double), Description = "O valor em Fahrenheit para conversao em celsius")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Retorna o valor em Celsius")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ConverterFahrenheitParaCelsius/{fahrenheit}")] HttpRequest req, double fahrenheit)
    {
      _logger.LogInformation($"Parametro recebido : {fahrenheit}", fahrenheit);

      var valorEmCelsius = (fahrenheit - 32) * 5 / 9;

      var resultado = new
      {
        valorEmCelsius = valorEmCelsius
      };

      _logger.LogInformation($"conversao efetuada, Resultado : {valorEmCelsius}", valorEmCelsius);

      return new JsonResult(resultado);
    }
  }
}

