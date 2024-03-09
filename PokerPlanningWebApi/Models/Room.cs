namespace PokerPlanningWebApi.Models;

public class Room : Base
{
    public string? Name { get; set; }
    public Guest? Admin { get; set; }
    public int? RevealScore { get; set; }
    public IList<string>? GuestsIds { get; set; }
}