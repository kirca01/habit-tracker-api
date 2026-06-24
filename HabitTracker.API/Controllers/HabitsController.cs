using HabitTracker.API.Data;
using HabitTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HabitTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HabitsController : ControllerBase
{
    private readonly AppDbContext _context;

    public HabitsController(AppDbContext context)
    {
        _context = context;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var habits = await _context.Habits
            .Where(h => h.UserId == GetUserId() && !h.IsArchived)
            .Include(h => h.Entries)
            .ToListAsync();

        return Ok(habits);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HabitDto dto)
    {
        var habit = new Habit
        {
            Name = dto.Name,
            Description = dto.Description,
            Color = dto.Color ?? "#6366f1",
            WeeklyGoal = dto.WeeklyGoal ?? 7,
            UserId = GetUserId()
        };

        _context.Habits.Add(habit);
        await _context.SaveChangesAsync();

        return Ok(habit);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var habit = await _context.Habits
            .FirstOrDefaultAsync(h => h.Id == id && h.UserId == GetUserId());

        if (habit == null) return NotFound();

        habit.IsArchived = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/checkin")]
    public async Task<IActionResult> CheckIn(int id)
    {
        var habit = await _context.Habits
            .FirstOrDefaultAsync(h => h.Id == id && h.UserId == GetUserId());

        if (habit == null) return NotFound();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var exists = await _context.HabitEntries
            .AnyAsync(e => e.HabitId == id && e.Date == today);

        if (exists) return BadRequest("Habit already checked in for today.");

        var entry = new HabitEntry { HabitId = id, Date = today };
        _context.HabitEntries.Add(entry);
        await _context.SaveChangesAsync();

        return Ok(entry);
    }

    [HttpDelete("{id}/checkin")]
    public async Task<IActionResult> UndoCheckIn(int id)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var entry = await _context.HabitEntries
            .FirstOrDefaultAsync(e => e.HabitId == id && e.Date == today);

        if (entry == null) return NotFound();

        _context.HabitEntries.Remove(entry);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] HabitDto dto)
    {
        var habit = await _context.Habits.FirstOrDefaultAsync(h => h.Id == id && h.UserId == GetUserId());

        if (habit == null) return NotFound();

        habit.Name = dto.Name;
        habit.Description = dto.Description;
        habit.Color = dto.Color ?? "#6366f1";
        habit.WeeklyGoal = dto.WeeklyGoal ?? 7;

        await _context.SaveChangesAsync();

        return Ok(habit);
    }
}

public record HabitDto(string Name, string? Description, string? Color, int? WeeklyGoal);