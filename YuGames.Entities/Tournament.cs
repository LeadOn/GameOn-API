namespace YuGames.Entities;

public class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int State { get; set; }
    public string? LogoUrl { get; set; }
    public DateTime PlannedFrom { get; set; }
    public DateTime PlannedTo { get; set; }
}