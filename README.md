# Big12 Memory App 🎯

A memory-keeping application built with clean architecture principles. Store your precious moments with photos and videos in a secure, well-structured way.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with a layered approach:

```
Big12MemoryApp/
├── Big12MemoryApp.API/              # Presentation layer (Controllers, Middleware)
├── Big12MemoryApp.Application/      # Application layer (Services, DTOs)
├── Big12MemoryApp.Domain/           # Domain layer (Entities, Repositories)
└── Big12MemoryApp.Infrastructure/   # Infrastructure layer (Database, External Services)
```

### Layer Responsibilities

- **API Layer**: REST API endpoints, authentication, request/response handling
- **Application Layer**: Business logic, DTOs, service implementations
- **Domain Layer**: Core entities, repository interfaces, domain services
- **Infrastructure Layer**: Database implementations, external service integrations

## 🚀 Tech Stack

### Backend
- **.NET 9.0** - Latest C# framework
- **ASP.NET Core** - Web API
- **Entity Framework Core 9.0** - ORM
- **PostgreSQL** - Relational database
- **JWT Authentication** - Secure authentication

### Cloud & Storage
- **Backblaze B2** - S3-compatible object storage
- **AWS SDK for S3** - File management

### Other
- **BCrypt.Net** - Password hashing
- **Swagger/OpenAPI** - API documentation
- **SMTP (Gmail)** - Email notifications

## ✨ Features

- JWT-based authentication with refresh tokens
- Create and manage memories with descriptions and dates
- Multi-file upload support (photos/videos)
- Add captions to attachments
- Cloud storage integration via Backblaze B2
- Birthday reminder system with email notifications
- Soft delete pattern for data recovery
- Clean RESTful API design  

## 🛠️ Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 12+](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/downloads)

### Installation

**1. Clone the repository**

```bash
git clone <repository-url>
cd Big12MemoryApp
```

**2. Set up the database**

Install PostgreSQL and create a database:

```sql
CREATE DATABASE big12memorydb;
```

**3. Configure the application**

Create an `appsettings.json` file in the `Big12MemoryApp.API` folder:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=big12memorydb;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "Key": "your-secret-key-min-32-characters-long",
    "Issuer": "Big12MemoryApp",
    "Audience": "Big12MemoryApp",
    "ExpireMinutes": 60,
    "RefreshTokenExpireDays": 7
  },
  "B2Storage": {
    "AccessKey": "your-b2-access-key",
    "SecretKey": "your-b2-secret-key",
    "BucketName": "your-bucket-name",
    "ServiceUrl": "https://s3.us-west-000.backblazeb2.com",
    "PublicBaseUrl": "https://f000.backblazeb2.com/file/your-bucket-name"
  },
  "GmailOptions": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Email": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```


**4. Apply database migrations**

```bash
cd Big12MemoryApp.API
dotnet ef database update
```

**5. Run the application**

```bash
dotnet run
```

The API will be available at `http://localhost:8080`

Access Swagger UI at: `http://localhost:8080/swagger`

## 📡 API Endpoints

### Authentication
```http
POST   /api/auth/login          # User login
POST   /api/auth/logout         # User logout
```

### Memories
```http
GET    /api/memories/my-memories              # Get user's memories
GET    /api/memories/{id}                     # Get memory details
POST   /api/memories                          # Create new memory
DELETE /api/memories/{id}                     # Delete memory
POST   /api/memories/{id}/attachments         # Add files to memory
DELETE /api/memories/{id}/attachments/{id}    # Remove file from memory
```

## 🗄️ Database Schema

**Users**
- User information (name, surname, email, birthday)
- Hashed password using BCrypt

**Memories**
- Memory description and date
- UserId (Foreign Key)

**Attachments**
- File metadata (name, type, size, storage path)
- UploadedByUserId (Foreign Key)

**MemoryAttachments**
- Many-to-many relationship table
- DisplayOrder and Caption for each attachment

**RefreshTokens**
- JWT refresh token management

## 🔐 Security

- JWT Bearer Authentication
- BCrypt password hashing
- Refresh token mechanism
- Authorization-ready architecture
- HTTPS enforcement (production)

## 📦 Dependencies

Key NuGet packages used:

```xml
<!-- API Layer -->
Microsoft.AspNetCore.Authentication.JwtBearer (9.0.1)
Swashbuckle.AspNetCore (6.6.2)

<!-- Application Layer -->
AWSSDK.S3 (4.0.15.1)

<!-- Domain Layer -->
BCrypt.Net-Next (4.0.3)

<!-- Infrastructure Layer -->
Npgsql.EntityFrameworkCore.PostgreSQL (9.0.1)
```

## 🔄 Background Jobs

The application uses `TimerService` for scheduled tasks:

- **Birthday Reminders**: Runs daily at 2:50 PM for now
- Checks for users with birthdays
- Sends email notifications to other users

## 🌐 Cloud Storage Setup (Backblaze B2)

Backblaze B2 is an S3-compatible object storage service:

1. Create a [Backblaze account](https://www.backblaze.com/b2/sign-up.html)
2. Create a bucket (public or private)
3. Generate an Application Key
4. Add credentials to `appsettings.json`

## 📧 Email Service Setup

Using Gmail SMTP. To set up:

1. Enable 2FA on your Gmail account
2. Generate an [App Password](https://myaccount.google.com/apppasswords)
3. Add the generated password to `appsettings.json`

## 🐳 Docker (Optional)

```bash
# Build Docker image
docker build -t big12memoryapp .

# Run container
docker run -p 8080:8080 big12memoryapp
```

## 📝 Development

### Adding New Migrations

```bash
dotnet ef migrations add MigrationName --project Big12MemoryApp.Infrastructure
dotnet ef database update --project Big12MemoryApp.API
```

### Creating a Test User

Manually insert a test user into your database:

```sql
INSERT INTO "Users" ("Name", "Surname", "Email", "Password", "Birthday", "IsDeleted", "CreatedAt", "ModifiedAt")
VALUES ('Test', 'User', 'test@example.com', '$2a$11$hashed_password', '1990-01-01', false, NOW(), NOW());
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'feat: Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 👥 Contact

Project Owner - [GitHub](https://github.com/cerencaglayan)

---

**Note**: This project is for educational and demonstration purposes. Conduct proper security testing before production use.
