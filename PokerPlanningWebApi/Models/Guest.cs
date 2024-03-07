namespace PokerPlanningWebApi.Models;

public class Guest : Base
{
    public int? Index { get; set; }
    public int? Score { get; set; }
    public string? ConnectionId { get; set; }
}