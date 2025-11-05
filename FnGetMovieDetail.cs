using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;

namespace HandsOnNetflixAzureServerless
{
    public class FnGetMovieDetail
    {
        private readonly ILogger<FnGetMovieDetail> _logger;
        private readonly CosmosClient _cosmosClient;

        public FnGetMovieDetail(ILogger<FnGetMovieDetail> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
        }

        [Function("GetMovieDetail")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "movies/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("Getting movie detail...");

            var container = _cosmosClient.GetContainer("NetflixDB", "Movies");

            if (string.IsNullOrEmpty(id))
            {
                return new BadRequestObjectResult("Movie id is required.");
            }

            try
            {
                var response = await container.ReadItemAsync<MovieResponse>(id, new PartitionKey(id));
                return new OkObjectResult(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new NotFoundObjectResult($"Movie with id '{id}' not found.");
            }
        }
    }
}