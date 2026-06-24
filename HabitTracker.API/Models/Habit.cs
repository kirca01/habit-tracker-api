namespace HabitTracker.API.Models;

public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#6366f1";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsArchived { get; set; } = false;
    public int WeeklyGoal { get; set; } = 7;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public ICollection<HabitEntry> Entries { get; set; } = new List<HabitEntry>();
}
