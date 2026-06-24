# Habit Tracker API

A RESTful API for tracking daily habits, built with ASP.NET Core 10.

## Tech Stack

- **ASP.NET Core 10** — Web API framework
- **Entity Framework Core** — ORM
- **PostgreSQL** — Database
- **JWT** — Authentication
- **BCrypt** — Password hashing

## Features

- User registration and login with JWT authentication
- Create, archive, and list habits
- Daily check-in and undo check-in
- Streak calculation logic
- Circular reference-safe JSON responses

## Getting Started

### Prerequisites

- .NET 10 SDK
- PostgreSQL (or Docker)

### Run with Docker

```bash
docker run --name habittracker-db \
  -e POSTGRES_PASSWORD=yourpassword \
  -e POSTGRES_DB=habittracker \
  -p 5432:5432 -d postgres
```

### Configuration

Create `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=habittracker;Username=postgres;Password=yourpassword"
  },
  "Jwt": {
    "Secret": "your-secret-key-minimum-32-characters",
    "ExpiryDays": 7
  }
}
```

### Run

```bash
dotnet ef database update
dotnet run
```

API will be available at `http://localhost:5230/swagger`

## Endpoints

| Method | Endpoint                 | Description             | Auth |
| ------ | ------------------------ | ----------------------- | ---- |
| POST   | /api/auth/register       | Register a new user     | No   |
| POST   | /api/auth/login          | Login and get JWT token | No   |
| GET    | /api/habits              | Get all habits          | Yes  |
| POST   | /api/habits              | Create a habit          | Yes  |
| DELETE | /api/habits/{id}         | Archive a habit         | Yes  |
| POST   | /api/habits/{id}/checkin | Check in for today      | Yes  |
| DELETE | /api/habits/{id}/checkin | Undo today's check-in   | Yes  |
