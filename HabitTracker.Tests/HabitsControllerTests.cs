using HabitTracker.API.Controllers;
using HabitTracker.API.Data;
using HabitTracker.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Xunit;

namespace HabitTracker.Tests;

public class HabitsControllerTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        return new AppDbContext(options);
    }

    private HabitsController GetControllerWithUser(AppDbContext context, int userId)
    {
        var controller = new HabitsController(context);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = principal}
        };

        return controller;
    }
    
    [Fact]
    public async Task Create_AddsHabitToDatabase()
    {
        var context = GetDbContext();
        var controller = GetControllerWithUser(context, userId: 1);
        var dto = new HabitDto("Exercise", "30 min", "#6366f1", 5);

        var result = await controller.Create(dto);

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(1, context.Habits.Count());
        Assert.Equal("Exercise", context.Habits.First().Name);
    }

    [Fact]
    public async Task GetAll_ReturnsOnlyusersHabits()
    {
        var context = GetDbContext();
        context.Habits.Add(new Habit { Name = "User1 Habit", UserId = 1});
        context.Habits.Add(new Habit { Name = "User2 Habit", UserId = 2});
        await context.SaveChangesAsync();

        var controller = GetControllerWithUser(context, userId: 1);
        var result = await controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var habits = Assert.IsAssignableFrom<List<Habit>>(okResult.Value);
        Assert.Single(habits);
        Assert.Equal("User1 Habit", habits.First().Name);
    }

    [Fact]
    public async Task CheckIn_CreatesEntryForToday()
    {
        var context = GetDbContext();
        context.Habits.Add(new Habit { Id = 1, Name = "Test", UserId = 1 });
        await context.SaveChangesAsync();

        var controller = GetControllerWithUser(context, userId: 1);
        var result = await controller.CheckIn(1);

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(1, context.HabitEntries.Count());
        Assert.Equal(DateOnly.FromDateTime(DateTime.UtcNow), context.HabitEntries.First().Date);
    }

    [Fact]
    public async Task CheckIn_Twice_ReturnsBadRequest()
    {
        var context = GetDbContext();
        context.Habits.Add(new Habit { Id = 1, Name = "Test", UserId = 1 });
        await context.SaveChangesAsync();

        var controller = GetControllerWithUser(context, userId: 1);
        await controller.CheckIn(1);
        var secondResult = await controller.CheckIn(1);

        Assert.IsType<BadRequestObjectResult>(secondResult);
        Assert.Equal(1, context.HabitEntries.Count());;
    }

    [Fact]
    public async Task Delete_ArchivesHabit()
    {
        var context = GetDbContext();
        context.Habits.Add(new Habit { Id = 1, Name = "Test", UserId = 1 });
        await context.SaveChangesAsync();

        var controller = GetControllerWithUser(context, userId: 1);
        var result = await controller.Delete(1);
        
        Assert.IsType<NoContentResult>(result);
        Assert.True(context.Habits.First().IsArchived);
    }
}