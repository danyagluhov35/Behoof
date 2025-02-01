namespace Behoof.IService
{
    public interface IFavoriteService
    {
        Task Add(string productId, string userId);
    }
}
