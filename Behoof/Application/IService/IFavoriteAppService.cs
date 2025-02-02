namespace Behoof.Application.IService
{
    public interface IFavoriteAppService
    {
        Task Add(string productId, string userId);
    }
}
