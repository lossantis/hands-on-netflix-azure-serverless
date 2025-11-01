using System.Reflection.Metadata;
using System.Security.Cryptography;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs.Models;

namespace HandsOnNetflixAzureServerless;

public class fnPostDataStorage
{
    private readonly ILogger<fnPostDataStorage> _logger;

    public fnPostDataStorage(ILogger<fnPostDataStorage> logger)
    {
        _logger = logger;
    }

    [Function("dataStorage")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("Processing request image...");

        try
        {
            if (!req.Headers.TryGetValue("file-type", out var fileTypeHeader))
            {
                return new BadRequestObjectResult("Missing 'file-type' header.");
            }
            var fileType = fileTypeHeader.ToString().ToLower();

            // Validate file type
            if (fileType != "image" && fileType != "video")
            {
                return new BadRequestObjectResult("Invalid 'file-type' header. Must be 'image' or 'video'.");
            }

            var form = await req.ReadFormAsync();
            var file = form.Files["file"];

            if (file == null || file.Length == 0)
            {
                return new BadRequestObjectResult("No file uploaded.");
            }

            string? connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            if (string.IsNullOrEmpty(connectionString))
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            string containerName = fileType == "image" ? "images" : "videos";

            BlobClient blobClient = new BlobClient(connectionString, containerName, file.FileName);
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            // Generate hash from filename
            string fileHash;
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(file.FileName + DateTime.UtcNow.Ticks));
                fileHash = Convert.ToHexString(hashBytes).ToLower();
            }

            // Get file extension from original filename
            string fileExtension = Path.GetExtension(file.FileName);

            // Create blob name using hash + extension
            string blobName = $"{fileHash}{fileExtension}";

            BlobClient blob = containerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
                await blob.UploadAsync(stream, true);
            }

            _logger.LogInformation($"File {file.FileName} uploaded to Blob Storage: {blob.Uri}");

            return new OkObjectResult(new
            {
                Message = $"File {file.FileName} uploaded successfully.",
                BlobUri = blob.Uri
            });

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing request: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}