using esok.api.Data;
using esok.api.DTO;

namespace esok.api.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductDTO>> GetUserProducts();
        Task<List<RentProduct>> GetRentProducts();
        Task<List<Rent>> GetAllRent(DateTime dateTimeFrom, DateTime dateTimeTo);
        Task<bool> HasPermission(List<int> productsId, int userId, int groupId);
        List<ProductDTO> GetProductsByRentId(int rentId);
        int CreateNumberRentOfDay(int groupId);
    }
}
