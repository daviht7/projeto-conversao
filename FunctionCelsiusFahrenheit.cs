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
  public class FunctionCelsiusFahrenheit
  {
    private readonly ILogger<FunctionCelsiusFahrenheit> _logger;

    public FunctionCelsiusFahrenheit(ILogger<FunctionCelsiusFahrenheit> log)
    {
      _logger = log;
    }

    [FunctionName("ConverterCelsiusParaFahrenheit")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "Conversao" })]
    [OpenApiParameter(name: "celsius", In = ParameterLocation.Path, Required = true, Type = typeof(double), Description = "O valor em Celsius para conversao em Fahrenheit")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Resultado), Description = "Retorna o valor em Fahrenheit")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ConverterCelsiusParaFahrenheit/{celsius}")] HttpRequest req, double celsius)
    {
      _logger.LogInformation($"Parametro recebido : {celsius}", celsius);

      var valorEmFahrenheit = ((celsius * 9) / 5) + 32;

      var resultado = new Resultado
      {
        ValorEmFahrenheit = valorEmFahrenheit
      };

      _logger.LogInformation($"conversao efetuada, Resultado : {valorEmFahrenheit}", valorEmFahrenheit);

      return new JsonResult(resultado);
    }
  }

  public class Resultado
  {

    public double ValorEmFahrenheit { get; set; }

  }

}

