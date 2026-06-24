# Habit Tracker

A full-stack habit tracking application with streaks, weekly goals, and a GitHub-style heatmap.

Backend built with ASP.NET Core 10, frontend with React + TypeScript.

> Frontend repo: [habit-tracker-client](https://github.com/kirca01/habit-tracker-client)

## Tech Stack

**Backend**
- ASP.NET Core 10 — Web API
- Entity Framework Core — ORM
- PostgreSQL — Database
- JWT — Authentication
- BCrypt — Password hashing
- xUnit — Unit testing

**DevOps**
- Docker & Docker Compose

## Features

- User registration and login with JWT authentication
- Create, edit, archive, and list habits
- Daily check-in and undo check-in
- Streak calculation
- Weekly goals (e.g. 5/7 days)
- Statistics (total check-ins, best streak, completion)
- Color-coded habits

## Getting Started

### Run with Docker Compose (recommended)

The easiest way to run the whole stack (API + database):

```bash
docker-compose up --build
```

API will be available at `http://localhost:5230/swagger`

Environment variables are read from a `.env` file:

```env
POSTGRES_PASSWORD=yourpassword
POSTGRES_DB=habittracker
JWT_SECRET=your-secret-key-minimum-32-characters
JWT_EXPIRY_DAYS=7
```

### Run locally (without Docker)

Requires .NET 10 SDK and a running PostgreSQL instance.

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

Then run:

```bash
cd HabitTracker.API
dotnet ef database update
dotnet run
```

## Running Tests

```bash
cd HabitTracker.Tests
dotnet test
```

## API Endpoints

| Method | Endpoint                 | Description             | Auth |
| ------ | ------------------------ | ----------------------- | ---- |
| POST   | /api/auth/register       | Register a new user     | No   |
| POST   | /api/auth/login          | Login and get JWT token | No   |
| GET    | /api/habits              | Get all habits          | Yes  |
| POST   | /api/habits              | Create a habit          | Yes  |
| PUT    | /api/habits/{id}         | Update a habit          | Yes  |
| DELETE | /api/habits/{id}         | Archive a habit         | Yes  |
| GET    | /api/habits/stats        | Get statistics          | Yes  |
| POST   | /api/habits/{id}/checkin | Check in for today      | Yes  |
| DELETE | /api/habits/{id}/checkin | Undo today's check-in   | Yes  |
