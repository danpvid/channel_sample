# Background Task Management with Channels in ASP.NET Core

This repository demonstrates how to use the `Channel` class in ASP.NET Core to manage background tasks. The example application simulates a customer import routine that runs in the background but is triggered via a controller endpoint. Additionally, it provides endpoints to monitor the status of individual imports and retrieve a list of all imports with their statuses.

## Features

- **Trigger Background Task:** Start a customer import process via a `POST` endpoint.
- **Check Import Status:** Retrieve the status of an import by its unique ID via a `GET` endpoint.
- **List All Imports:** Get a list of all imports with their respective statuses via another `GET` endpoint.

## Technology Stack

- **ASP.NET Core**: Web API framework.
- **System.Threading.Channels**: For managing background task queues.
- **In-Memory Storage**: To keep track of import statuses (for demonstration purposes).

## How It Works

1. **POST `/api/import`**: This endpoint enqueues a customer import task and returns a unique import ID. The task is processed in the background by a hosted service using the `Channel` class.

2. **GET `/api/import/{id}`**: This endpoint retrieves the status of a specific import task using its unique ID.

3. **GET `/api/imports`**: This endpoint returns a list of all imports along with their statuses.

### Task Workflow

- The `Channel` class is used to queue tasks for background processing.
- A `BackgroundService` continuously monitors the channel, processes tasks, and updates their statuses in the in-memory storage.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later

### Running the Application

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/channel-import-demo.git
   cd channel-import-demo
   ```

2. Build and run the application:
   ```bash
   dotnet run
   ```

3. Use a tool like [Postman](https://www.postman.com/) or `curl` to interact with the API.

### API Endpoints

#### Trigger an Import

- **Endpoint:** `POST /api/import`
- **Request Body:**
  ```json
  {
    "filePath": "path/to/customer-data.csv"
  }
  ```
- **Response:**
  ```json
  {
    "importId": "unique-import-id"
  }
  ```

#### Check Import Status

- **Endpoint:** `GET /api/import/{id}`
- **Response:**
  ```json
  {
    "importId": "unique-import-id",
    "status": "Processing"
  }
  ```

#### List All Imports

- **Endpoint:** `GET /api/imports`
- **Response:**
  ```json
  [
    {
      "importId": "unique-import-id",
      "status": "Completed"
    },
    {
      "importId": "another-import-id",
      "status": "Processing"
    }
  ]
  ```

## Project Structure

- **Controllers/**: Contains API controllers.
- **Services/**: Includes the background service and related logic for task processing.
- **Models/**: Defines data models for imports and statuses.

## Contributing

Contributions are welcome! Please fork this repository and submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

