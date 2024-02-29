namespace PokerPlanningWebApi.Models;

public class Room : Base
{
    public string? Name { get; set; }
    public Guest? Admin { get; set; }
    public IList<Guest>? Guests { get; set; }
}