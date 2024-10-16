namespace Identity.API.Domain.Abstractions
{
    public interface IDateTracking
    {
        DateTime? CreatedAt { get; set; }
        DateTime? LastModified { get; set; }
    }
}
