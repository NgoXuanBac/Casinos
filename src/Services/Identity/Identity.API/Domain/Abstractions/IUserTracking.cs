namespace Identity.API.Domain.Abstractions
{
    public interface IUserTracking
    {
        string? CreatedBy { get; set; }
        string? LastModifiedBy { get; set; }
    }
}