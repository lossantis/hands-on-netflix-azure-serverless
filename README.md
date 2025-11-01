# Netflix Azure Serverless Challenge

A serverless Azure Functions application built with .NET 9 for handling media file uploads and storage, designed to simulate a Netflix-like content management system.

## 🏗️ Architecture Overview

This project demonstrates a cloud-native serverless architecture using Azure Functions for media file processing and storage management. The application provides RESTful endpoints for uploading images and videos to Azure Blob Storage.

## 📁 Project Structure

```
HandsOnNetflixAzureServerless/
├── fnPostDataStorage.cs           # HTTP-triggered Azure Function for file uploads
├── Program.cs                     # Application entry point and configuration
├── HandsOnNetflixAzureServerless.csproj  # Project file with dependencies
├── HandsOnNetflixAzureServerless.sln     # Solution file
├── host.json                      # Azure Functions host configuration
├── local.settings.json            # Local development settings
├── Properties/
│   └── launchSettings.json        # Launch profiles for development
├── Public/
│   ├── Images/                    # Static images directory
│   └── Videos/                    # Static videos directory
├── bin/                           # Build output directory
└── obj/                           # Build intermediate files
```

## 🚀 Core Features

### Data Storage Function (`fnPostDataStorage`)

- **Endpoint**: HTTP POST `/api/dataStorage`
- **Authorization**: Function-level authorization required
- **Functionality**:
  - Accepts file uploads with configurable file types (image/video)
  - Validates file type via `file-type` header
  - Generates unique file names using SHA256 hash
  - Uploads files to appropriate Azure Blob Storage containers
  - Returns upload confirmation with blob URI

### Key Capabilities

1. **File Type Validation**: Supports both images and videos with header-based routing
2. **Dynamic Container Creation**: Automatically creates blob containers if they don't exist
3. **Hash-based Naming**: Generates unique file names to prevent conflicts
4. **Public Access**: Configures blob containers for public access
5. **Error Handling**: Comprehensive error handling and logging

## 🔧 Technical Stack

- **.NET 9.0**: Latest .NET framework for optimal performance
- **Azure Functions v4**: Serverless compute platform
- **Azure Blob Storage**: Scalable object storage for media files
- **ASP.NET Core**: Web framework integration
- **Application Insights**: Monitoring and telemetry

### Dependencies

- `Azure.Storage.Blobs` (v12.26.0) - Azure Blob Storage client library
- `Microsoft.Azure.Functions.Worker` (v2.1.0) - Azure Functions worker runtime
- `Microsoft.ApplicationInsights.WorkerService` (v2.23.0) - Application monitoring
- `Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore` (v2.1.0) - HTTP integration

## ☁️ Azure Resources

### Resource Group
- **Name**: `netflix-azure-challenge`
- **Purpose**: Container for all Netflix challenge resources

### API Management (APIM)
- **Name**: `apim-netflix-azure-challenge`
- **Purpose**: 
  - API gateway for function endpoints
  - Rate limiting and throttling
  - API versioning and documentation
  - Security policies and authentication

### Database
- **Name**: `cosmosdbnetflix`
- **Type**: Azure Cosmos DB
- **Purpose**: 
  - NoSQL database for metadata storage
  - User profiles and preferences
  - Content catalog and indexing
  - Global distribution capabilities

### Storage Account
- **Name**: `stanetflixdev001`
- **Purpose**: 
  - Primary storage for media files (images/videos)
  - Blob containers: `images` and `videos`
  - Static website hosting for public content
  - CDN integration for global content delivery

## 🎯 Use Cases

### 1. Media Upload Service
- **Scenario**: Content creators uploading movie thumbnails and trailers
- **Flow**: 
  1. Client sends POST request with file and `file-type` header
  2. Function validates file type and format
  3. File is uploaded to appropriate blob container
  4. Unique URL is returned for content access

### 2. Content Management System
- **Scenario**: Admin dashboard managing media assets
- **Benefits**:
  - Scalable storage without infrastructure management
  - Automatic file organization by type
  - Public URLs for direct content access

### 3. Microservices Architecture
- **Scenario**: Part of larger Netflix-like platform
- **Integration**:
  - APIM routes requests to appropriate functions
  - Cosmos DB stores metadata and relationships
  - Storage Account provides CDN-ready content delivery

## 🛠️ Development Setup

### Prerequisites
- .NET 9.0 SDK
- Azure Functions Core Tools
- Azure Storage Emulator or Azure Storage Account
- Visual Studio Code or Visual Studio 2022

### Local Development

1. **Clone and Build**:
   ```bash
   git clone <repository-url>
   cd HandsOnNetflixAzureServerless
   dotnet build
   ```

2. **Configure Local Settings**:
   Update `local.settings.json` with your Azure Storage connection string:
   ```json
   {
     "IsEncrypted": false,
     "Values": {
       "AzureWebJobsStorage": "DefaultEndpointsProtocol=https;AccountName=stanetflixdev001;AccountKey=...",
       "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
     }
   }
   ```

3. **Run Locally**:
   ```bash
   func start
   ```

### Available Tasks
- `build (functions)`: Build the project
- `clean (functions)`: Clean build artifacts
- `publish (functions)`: Publish for deployment
- `func: 4`: Start local function host

## 📡 API Usage

### Upload Image
```bash
curl -X POST "http://localhost:7071/api/dataStorage" \
  -H "Content-Type: multipart/form-data" \
  -H "file-type: image" \
  -F "file=@movie-poster.jpg"
```

### Upload Video
```bash
curl -X POST "http://localhost:7071/api/dataStorage" \
  -H "Content-Type: multipart/form-data" \
  -H "file-type: video" \
  -F "file=@movie-trailer.mp4"
```

### Response Format
```json
{
  "Message": "File movie-poster.jpg uploaded successfully.",
  "BlobUri": "https://stanetflixdev001.blob.core.windows.net/images/a1b2c3d4e5f6...jpg"
}
```

## 🔍 Monitoring and Logging

- **Application Insights**: Integrated for performance monitoring and error tracking
- **Structured Logging**: Comprehensive logging throughout the application
- **Live Metrics**: Real-time performance monitoring in Azure portal

## 🚀 Deployment

### Azure Deployment
```bash
# Build and publish
dotnet publish --configuration Release

# Deploy to Azure (using Azure CLI)
az functionapp deployment source config-zip \
  --resource-group netflix-azure-challenge \
  --name your-function-app-name \
  --src publish.zip
```

## 🔒 Security Considerations

- Function-level authorization required for API access
- Environment variables for sensitive configuration
- Public blob access configured for content delivery
- CORS policies configurable via host.json

## 📈 Scalability Features

- **Serverless Architecture**: Automatic scaling based on demand
- **Blob Storage**: Virtually unlimited storage capacity
- **CDN Integration**: Global content delivery optimization
- **Container Organization**: Efficient file organization and retrieval

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is part of the Azure AZ-204 certification hands-on learning experience.

---

**Last Updated**: November 2025  
**Azure Functions Version**: v4  
**.NET Version**: 9.0