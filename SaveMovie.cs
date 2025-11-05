using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HandsOnNetflixAzureServerless
{
    public class SaveMovie
    {
        private readonly ILogger<SaveMovie> _logger;

        public SaveMovie(ILogger<SaveMovie> logger)
        {
            _logger = logger;
        }

        [Function("SaveMovie")]
        [CosmosDBOutput("%CosmosDBDatabaseName%", "Movies", Connection = "CosmosDBConnectionString")]
        public async Task<object?> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "movies")] HttpRequestData req)
        {
            _logger.LogInformation("Saving movie...");

            MovieRequest? movie = null;

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            _logger.LogInformation($"Request body received: {content}");

            try
            {
                movie = JsonConvert.DeserializeObject<MovieRequest>(content);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during deserialization.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            }

            return JsonConvert.SerializeObject(movie);
        }
    }
}