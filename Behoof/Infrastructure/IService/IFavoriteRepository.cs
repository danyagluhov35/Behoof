namespace Behoof.Infrastructure.IService
{
    public interface IFavoriteRepository
    {
        Task Add(string productId, string userId);
    }
}
