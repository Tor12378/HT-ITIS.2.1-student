namespace Models;

public class FightResult : Fight
{
    public string? Message { get; set; }
    public bool IsRoundEnd { get; set; }
}