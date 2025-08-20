using AutoMapper;
using esok.api.Application.Common.Enum;
using esok.api.Data;
using esok.api.DTO;
using esok.api.Interfaces.Repository;
using esok.api.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace esok.api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext applicationDbContext,
            IAuthenticationService authenticationService,
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<List<ProductDTO>> GetUserProducts()
        {
            var user = await _authenticationService
                .GetUser();

            var products = _applicationDbContext
                .Product
                .Where(x => x.GroupId == user.GroupId && x.Active == true)
                .ToList();

            var result = _mapper
                .Map<List<ProductDTO>>(products);    

            return result;
        }

        public async Task<List<RentProduct>> GetRentProducts()
        {
            var user = await _authenticationService
                .GetUser();

            var rent = _applicationDbContext
                .Rent
                .Where(x => x.GroupId == user.GroupId && x.Active == true)
                .Select(s => s.Id)
                .ToList();

            var rentProducts = _applicationDbContext
                .RentProduct
                .Where(x => rent.Contains(x.RentId) && x.Active == true)
                .ToList();

            return rentProducts;
        }

        public async Task<List<Rent>> GetAllRent(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var user = await _authenticationService
                .GetUser();

            return _applicationDbContext
                .Rent
                .Where(x => x.GroupId == user.GroupId && x.CreateDate >= dateTimeFrom && x.CreateDate < dateTimeTo)
                .ToList();
        }

        public async Task<bool> HasPermission(List<int> productsId, int userId, int groupId)
        {
            var isGroupManager = await _applicationDbContext
                .UserRoles
                .Where(x => x.UserId == userId && x.RoleId == (int) Role.Manager)
                .AnyAsync();

            if (isGroupManager)
                return true;

            var hasPermission = await _applicationDbContext
                .Product
                .Where(x => productsId.Contains(x.Id) && x.GroupId == groupId)
                .AnyAsync();

            return hasPermission;
        }

        public List<ProductDTO> GetProductsByRentId(int rentId)
        {
            var products = (from rp in _applicationDbContext.RentProduct.AsQueryable()
                    join p in _applicationDbContext.Product.AsQueryable() on rp.ProductId equals p.Id
                    where rp.RentId == rentId
                    select new ProductDTO
                    {
                        Name = p.Name,
                        Description = p.Description,
                        NetPrice = p.NetPrice,
                        Quanity = rp.Quanity,
                        Unit = p.Unit
                    }).ToList();

            return products;
        }

        public int CreateNumberRentOfDay(int groupId)
        {
            var dateNow = DateTime.Now.Date;
            var dateTimeFrom = dateNow.ToUniversalTime();
            var dateTimeTo = dateTimeFrom.AddDays(1);

            var dailyCount = _applicationDbContext
                .Rent
                .Where(x => x.GroupId == groupId && x.CreateDate >= dateTimeFrom && x.CreateDate < dateTimeTo)
                .Count();

            return dailyCount += 1;
        }
    }
}
