namespace Identity.API.Domain.Abstractions
{
    public interface IAuditable : IDateTracking, IUserTracking;
}