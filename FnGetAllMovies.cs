using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace HandsOnNetflixAzureServerless
{
    public class FnGetAllMovies
    {
        private readonly ILogger<FnGetAllMovies> _logger;
        private readonly CosmosClient _cosmosClient;

        public FnGetAllMovies(ILogger<FnGetAllMovies> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
        }

        [Function("GetAllMovies")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "movies")] HttpRequestData req)
        {
            _logger.LogInformation("Getting all movies...");

            var container = _cosmosClient.GetContainer("NetflixDB", "Movies");

            try
            {
                var query = "SELECT * FROM c";
                var queryDefinition = new QueryDefinition(query);
                var queryResultSetIterator = container.GetItemQueryIterator<MovieResponse>(queryDefinition);

                var movies = new List<MovieResponse>();

                while (queryResultSetIterator.HasMoreResults)
                {
                    var queryResponse = await queryResultSetIterator.ReadNextAsync();
                    movies.AddRange(queryResponse.ToList());
                }

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(movies);
                return response;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving movies");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                return errorResponse;
            }
        }
    }
}