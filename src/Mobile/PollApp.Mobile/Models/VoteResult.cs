public class VoteResult
{
    public int PollId { get; set; }
    public List<OptionResult> OptionResults { get; set; }
}

public class OptionResult
{
    public int OptionId { get; set; }
    public string OptionText { get; set; }
    public int VoteCount { get; set; }
    public double Percentage { get; set; }
}