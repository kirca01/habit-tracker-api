namespace HabitTracker.API.Models;

public class HabitEntry
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    public int HabitId { get; set; }
    public Habit Habit { get; set; } = null!; 
}