namespace OnlinePollSystem.Domain.Enums
{
    public enum PollStatus
    {
        Draft = 0,       // Poll is being created
        Active = 1,      // Poll is open for voting
        Closed = 2,      // Poll has ended
        Archived = 3     // Poll is no longer active
    }
}