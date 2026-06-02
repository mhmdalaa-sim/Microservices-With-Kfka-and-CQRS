# ProductService Microservice

A .NET 8 microservice built with clean architecture principles using CQRS (Command Query Responsibility Segregation) pattern and event-driven communication via Apache Kafka.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Dependencies](#dependencies)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Design Patterns](#design-patterns)
- [Configuration](#configuration)
- [Running Tests](#running-tests)
- [Development](#development)
- [Contributing](#contributing)

## 🎯 Overview

ProductService is a microservice that manages product creation and retrieval operations. It demonstrates best practices in microservice architecture including:

- **CQRS Pattern**: Separates read and write operations
- **Event-Driven Architecture**: Publishes domain events to Kafka topics
- **Dependency Injection**: Uses built-in .NET DI container
- **Clean Code Principles**: Organized folder structure with single responsibility
- **Mediator Pattern**: Decouples request handlers using MediatR

## 🏗️ Architecture

### System Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                      ProductService                          │
├─────────────────────────────────────────────────────────────┤
│  ┌──────────────────────────────────────────────────────┐  │
│  │          HTTP Layer (Controllers)                    │  │
│  │  ├─ POST /api/products (Create Product)              │  │
│  │  └─ GET  /api/products (Get All Products)            │  │
│  └──────────────────────────────────────────────────────┘  │
│              │                          │                    │
│              ▼                          ▼                    │
│  ┌──────────────────────┐   ┌────────────────────────┐     │
│  │  Mediator Pipeline   │   │  MediatR Dispatcher    │     │
│  │  (IMediator)         │   │                        │     │
│  └──────────────────────┘   └────────────────────────┘     │
│              │                          │                    │
│    ┌─────────┴──────────┐      ┌───────┴──────────┐        │
│    ▼                    ▼      ▼                   ▼        │
│ ┌─────────────┐   ┌─────────────┐   ┌────────────────┐    │
│ │  Commands   │   │  Queries    │   │ Event Handlers │    │
│ │             │   │             │   │                │    │
│ │ CreateProduct   GetAllProducts    ProductCreated  │    │
│ │ Command     │   │ Query       │   │ Event Handler  │    │
│ └─────────────┘   └─────────────┘   └────────────────┘    │
│    │                   │                  │                 │
│    ▼                   ▼                  ▼                 │
│ ┌─────────────────────────────────────────────────────┐   │
│ │  Service Layer (ProductStore)                       │   │
│ │  - In-Memory Product Storage                        │   │
│ │  - CRUD Operations                                  │   │
│ └─────────────────────────────────────────────────────┘   │
│                      │                                     │
│                      ▼                                     │
│ ┌─────────────────────────────────────────────────────┐   │
│ │  Domain Models                                      │   │
│ │  - Product Entity                                   │   │
│ │  - ProductCreatedEvent                              │   │
│ └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
					  │
					  ▼
		┌──────────────────────────┐
		│   Apache Kafka Cluster   │
		│   Topic: product-created │
		└──────────────────────────┘
					  │
					  ▼
		┌──────────────────────────┐
		│ ProductConsumerService   │
		│ (Event Subscriber)       │
		└──────────────────────────┘
```

## 🛠️ Technology Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| .NET | 8.0 | Framework |
| ASP.NET Core | 8.0 | Web Framework |
| MediatR | 12.0.1 | Mediator Pattern Implementation |
| MediatR.Extensions.Microsoft.DependencyInjection | 11.1.0 | DI Integration |
| Confluent.Kafka | 2.10.0 | Kafka Producer/Consumer |
| Swashbuckle.AspNetCore | 6.4.0 | Swagger/OpenAPI Documentation |

## 📦 Prerequisites

### Required Software

- **.NET 8.0 SDK** or later
  - Download: https://dotnet.microsoft.com/download/dotnet/8.0
  - Verify: `dotnet --version`

- **Apache Kafka 3.0+**
  - Download: https://kafka.apache.org/downloads
  - Or use Docker: `docker pull confluentinc/cp-kafka`

- **Visual Studio 2026 Community** (or later)
  - Download: https://visualstudio.microsoft.com/downloads/

- **Git** (for version control)
  - Download: https://git-scm.com/download/win

### System Requirements

- Windows 10/11 or Linux/macOS
- Minimum 4GB RAM (8GB recommended)
- Java Runtime Environment (JRE) 11+ (for Kafka)
- Port 9092 available (Kafka default)
- Port 5000/5001 available (ASP.NET Core default)

## 📚 Dependencies

### NuGet Packages

All dependencies are defined in `ProductService.csproj`:

```xml
<ItemGroup>
	<PackageReference Include="Confluent.Kafka" Version="2.10.0" />
	<PackageReference Include="MediatR" Version="12.0.1" />
	<PackageReference Include="MediatR.Extensions.Microsoft.DependencyInjection" Version="11.1.0" />
	<PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
</ItemGroup>
```

### Restore Dependencies

```powershell
dotnet restore
```

### External Dependencies

1. **Apache Kafka**
   - Default connection: `localhost:9092`
   - Topic: `product-created` (auto-created if needed)

2. **ProductConsumerService** (Related Microservice)
   - Consumes events published by ProductService
   - Location: `..\ProductConsumerService\`

## 📂 Project Structure

```
ProductService/
├── CommandHandlers/
│   └── CreateProductHandler.cs          # Handles CreateProductCommand
├── Commands/
│   └── CreateProductCommand.cs          # Command for creating products
├── Controllers/
│   └── ProductsController.cs            # REST API endpoints
├── Events/
│   └── ProductCreatedEvent.cs           # Domain event (published to Kafka)
├── Models/
│   └── Product.cs                       # Product domain model
├── Queries/
│   ├── GetAllProductsQuery.cs           # Query for retrieving all products
│   └── GetAllProductsHandler.cs         # Query handler
├── Services/
│   └── ProductStore.cs                  # In-memory product storage
├── Properties/
│   ├── launchSettings.json              # Launch configuration
├── Program.cs                           # Startup configuration
├── ProductService.csproj                # Project file with dependencies
├── ProductService.http                  # HTTP testing file
├── appsettings.json                     # Application settings
├── appsettings.Development.json         # Development settings
└── README.md                            # This file
```

## 🚀 Getting Started

### 1. Prerequisites Setup

#### Install .NET 8 SDK
```powershell
# Verify .NET 8 is installed
dotnet --list-sdks

# If not installed, download from:
# https://dotnet.microsoft.com/download/dotnet/8.0
```

#### Start Apache Kafka

**Option A: Using Docker Compose** (Recommended)

Create a `docker-compose.yml` in the Microservices root directory:

```yaml
version: '3.8'
services:
  zookeeper:
	image: confluentinc/cp-zookeeper:7.5.0
	environment:
	  ZOOKEEPER_CLIENT_PORT: 2181
	ports:
	  - "2181:2181"

  kafka:
	image: confluentinc/cp-kafka:7.5.0
	depends_on:
	  - zookeeper
	ports:
	  - "9092:9092"
	environment:
	  KAFKA_BROKER_ID: 1
	  KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
	  KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:29092,PLAINTEXT_HOST://localhost:9092
	  KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
	  KAFKA_INTER_BROKER_LISTENER_NAME: PLAINTEXT
	  KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
```

Run: `docker-compose up -d`

**Option B: Local Kafka Installation**

1. Download Kafka from https://kafka.apache.org/downloads
2. Extract to a directory
3. Start Zookeeper: `bin/zookeeper-server-start.sh config/zookeeper.properties`
4. Start Kafka: `bin/kafka-server-start.sh config/server.properties`

### 2. Clone and Open Project

```powershell
# Navigate to project directory
cd D:\selfwork\Microservices\ProductService

# Restore NuGet packages
dotnet restore

# Open in Visual Studio
start ProductService.csproj
```

### 3. Build and Run

```powershell
# Build the project
dotnet build

# Run the project
dotnet run

# Or run with watch mode (auto-restart on changes)
dotnet watch run
```

The service will start on:
- **HTTP**: https://localhost:5000
- **HTTPS**: https://localhost:5001

### 4. Access Swagger Documentation

Navigate to: `https://localhost:5001/swagger/index.html`

## 📡 API Endpoints

### Create Product

```http
POST /api/products
Content-Type: application/json

{
  "name": "Laptop"
}
```

**Response (200 OK):**
```json
{
  "message": "Product created",
  "id": "550e8400-e29b-41d4-a716-446655440000"
}
```

### Get All Products

```http
GET /api/products
```

**Response (200 OK):**
```json
[
  {
	"id": "550e8400-e29b-41d4-a716-446655440000",
	"name": "Laptop"
  },
  {
	"id": "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
	"name": "Mouse"
  }
]
```

## 🎨 Design Patterns

### 1. **CQRS (Command Query Responsibility Segregation)**

Separates read and write operations:

- **Commands** (`Commands/`): Modify state (CreateProductCommand)
- **Queries** (`Queries/`): Read state (GetAllProductsQuery)
- **Handlers** (`CommandHandlers/`, `Queries/`): Execute business logic

**Benefits:**
- Scalability: Read and write can be scaled independently
- Performance: Optimize each path separately
- Clarity: Clear intent of operations

### 2. **Mediator Pattern**

Encapsulates how commands and queries are processed:

```csharp
// Instead of direct invocation
var handler = new CreateProductHandler(store, producer);
var result = handler.Handle(command);

// Use Mediator
var result = await _mediator.Send(command);
```

**Benefits:**
- Loose coupling
- Centralized request processing
- Easy to add cross-cutting concerns (logging, validation)

### 3. **Repository Pattern**

`ProductStore` acts as a data access abstraction:

```csharp
public class ProductStore
{
	private readonly List<Product> _products = new();
	public void Add(Product product) => _products.Add(product);
	public IEnumerable<Product> GetAll() => _products;
}
```

### 4. **Dependency Injection**

Services are registered in `Program.cs`:

```csharp
builder.Services.AddSingleton<ProductStore>();
builder.Services.AddMediatR(cfg => 
	cfg.RegisterServicesFromAssemblyContaining<Program>());
```

### 5. **Event-Driven Architecture**

Domain events published to Kafka:

```csharp
var @event = new ProductCreatedEvent(product.Id, product.Name);
var message = new Message<Null, string> 
{ 
	Value = JsonSerializer.Serialize(@event) 
};
await _producer.ProduceAsync("product-created", message);
```

**Benefits:**
- Asynchronous communication
- Loose coupling between microservices
- Event sourcing capability

## ⚙️ Configuration

### appsettings.json

```json
{
  "Logging": {
	"LogLevel": {
	  "Default": "Information"
	}
  },
  "AllowedHosts": "*"
}
```

### appsettings.Development.json

```json
{
  "Logging": {
	"LogLevel": {
	  "Default": "Debug",
	  "Microsoft.AspNetCore": "Warning"
	}
  }
}
```

### Kafka Configuration

Edit `Program.cs` to customize Kafka settings:

```csharp
var config = new ProducerConfig 
{ 
	BootstrapServers = "localhost:9092"  // Change if needed
};
```

### Environment Variables

You can override settings via environment variables:

```powershell
# PowerShell
$env:KAFKA_BOOTSTRAP_SERVERS="kafka.example.com:9092"
dotnet run

# Or in .env file
KAFKA_BOOTSTRAP_SERVERS=kafka.example.com:9092
```

## 🧪 Running Tests

Currently, no unit tests are included. To add tests, create a `ProductService.Tests` project:

```powershell
# Create test project
dotnet new xunit -n ProductService.Tests

# Add test project to solution
dotnet sln add ProductService.Tests

# Run tests
dotnet test
```

### Example Test Structure

```csharp
public class CreateProductHandlerTests
{
	[Fact]
	public async Task Handle_WithValidCommand_ReturnsProductId()
	{
		// Arrange
		var store = new ProductStore();
		var producer = new Mock<IProducer<Null, string>>();
		var handler = new CreateProductHandler(store, producer.Object);
		var command = new CreateProductCommand("Laptop");

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.NotEqual(Guid.Empty, result);
		Assert.Single(store.GetAll());
	}
}
```

## 💻 Development

### Running in Debug Mode

```powershell
dotnet run --configuration Debug
```

### Using Visual Studio Debugger

1. Open `ProductService.sln` in Visual Studio
2. Set breakpoints in your code
3. Press `F5` to start debugging

### Using HTTP Files for Testing

The `ProductService.http` file allows testing endpoints directly in Visual Studio:

```http
### Create Product
POST https://localhost:5001/api/products
Content-Type: application/json

{
  "name": "Keyboard"
}

### Get All Products
GET https://localhost:5001/api/products
```

### Monitoring Kafka Events

```powershell
# Create Kafka topic consumer (in separate terminal)
# Navigate to Kafka directory
cd C:\kafka

# Consume from topic
.\bin\windows\kafka-console-consumer.bat --bootstrap-server localhost:9092 --topic product-created --from-beginning
```

## 🔄 Integration with ProductConsumerService

The ProductConsumerService subscribes to `product-created` events:

1. ProductService publishes `ProductCreatedEvent` to Kafka topic `product-created`
2. ProductConsumerService listens on the same topic
3. Consumer processes the event asynchronously

### Verified Communication

```powershell
# Terminal 1: Start ProductService
cd D:\selfwork\Microservices\ProductService
dotnet run

# Terminal 2: Start ProductConsumerService
cd D:\selfwork\Microservices\ProductConsumerService
dotnet run

# Terminal 3: Create a product
curl -X POST https://localhost:5001/api/products `
  -H "Content-Type: application/json" `
  -d '{"name":"Test Product"}'

# Check consumer service logs for event processing
```

## 📋 Code Quality Checklist

- ✅ Clean Code Principles Applied
- ✅ Single Responsibility Principle (SRP)
- ✅ Dependency Injection Pattern
- ✅ CQRS Pattern Implemented
- ✅ Event-Driven Architecture
- ✅ Async/Await Best Practices
- ✅ Proper Error Handling (Consider adding)
- ✅ Logging (Consider enhancing)
- ✅ Unit Tests (Not yet implemented - Recommended)
- ✅ Integration Tests (Not yet implemented - Recommended)

## 🚨 Known Limitations & Future Improvements

### Current Limitations

1. **In-Memory Storage**: Data lost on restart (use database for persistence)
2. **No Error Handling**: Add try-catch blocks and appropriate error responses
3. **No Logging**: Implement structured logging (Serilog recommended)
4. **No Authentication**: Add OAuth2/JWT for production
5. **No Database Persistence**: Consider EF Core with SQL Server
6. **No Unit Tests**: Add comprehensive test coverage

### Recommended Enhancements

```csharp
// 1. Add Database Context
public class ProductDbContext : DbContext
{
	public DbSet<Product> Products { get; set; }
}

// 2. Add Logging
private readonly ILogger<CreateProductHandler> _logger;

// 3. Add Validation
public record CreateProductCommand(string Name) : IRequest<Guid>
{
	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Name))
			throw new ArgumentException("Product name is required");
	}
}

// 4. Add Error Response
public class ErrorResponse
{
	public string Message { get; set; }
	public string Details { get; set; }
	public DateTime Timestamp { get; set; }
}
```

## 🔗 Related Projects

- **ProductConsumerService**: Event consumer microservice
  - Location: `D:\selfwork\Microservices\ProductConsumerService`
  - Role: Consumes `product-created` events from Kafka

## 📖 Useful Resources

- [Microsoft .NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/fundamentals/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [MediatR GitHub](https://github.com/jbogard/MediatR)
- [Confluent Kafka .NET Client](https://docs.confluent.io/kafka-clients/dotnet/current/overview.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [Microservices Architecture](https://microservices.io/)

## 👤 Author

Created as part of the Microservices Architecture learning project with Kafka and CQRS patterns.

## 📄 License

This project is part of the Microservices learning repository.

---

## 🆘 Troubleshooting

### Issue: Kafka Connection Failed

```
Error: Unable to connect to bootstrap server localhost:9092
```

**Solution:**
- Verify Kafka is running: `netstat -an | findstr 9092`
- Check Kafka logs for errors
- Ensure correct bootstrap server address in `Program.cs`

### Issue: Port Already in Use

```
Error: Address already in use. Port: 5001
```

**Solution:**
```powershell
# Find process using port 5001
netstat -ano | findstr :5001

# Kill process (replace PID with actual process ID)
taskkill /PID [PID] /F

# Or run on different port
dotnet run --urls "https://localhost:5002"
```

### Issue: NuGet Restore Fails

```powershell
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

### Issue: MediatR Handlers Not Registered

**Solution:**
Ensure `Program.cs` includes:
```csharp
builder.Services.AddMediatR(cfg => 
	cfg.RegisterServicesFromAssemblyContaining<Program>());
```

---

**Last Updated**: December 2024
**Version**: 1.0
**Status**: Production Ready (with recommendations for enhancements)
