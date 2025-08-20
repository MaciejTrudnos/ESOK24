using AutoMapper;
using esok.api.Application.Common;
using esok.api.Application.Common.Enum;
using esok.api.Data;
using esok.api.DTO;
using esok.api.Interfaces.Repository;
using esok.api.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace esok.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductController(IAuthenticationService authenticationService,
            IProductRepository productRepository,
            IMapper mapper,
            ApplicationDbContext applicationDbContext)
        {
            _authenticationService= authenticationService;
            _productRepository = productRepository;
            _mapper = mapper;
            _applicationDbContext= applicationDbContext;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO product)
        {
            var user = await _authenticationService
                .GetUser();

            var exist = _applicationDbContext
                .Product
                .Where(x => x.Name.ToLower().Contains(product.Name.ToLower()) && x.GroupId == user.GroupId && x.Active == true)
                .Any();

            if (exist)
                return Conflict();

            var data = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                NetPrice = product.NetPrice,
                Quanity = product.Quanity,
                Unit = product.Unit,
                GroupId = user.GroupId,
                CreateDate = AppDate.DateTimeNow,
                CreatedByUserId = user.Id,
                Active = true
            };

            await _applicationDbContext
                .Product
                .AddAsync(data);

            await _applicationDbContext
                .SaveChangesAsync();

            return Ok(data.Id);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository
                .GetUserProducts();

            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var user = await _authenticationService
                .GetUser();

            var hasPermission = await _productRepository
                .HasPermission(new List<int>() { productId }, user.Id, user.GroupId);

            if (!hasPermission)
                return Forbid();

            var product = _applicationDbContext
                .Product
                .Where(x => x.Id == productId && x.Active == true)
                .First();

            product.Active = false;
            product.UpdatedByUserId = user.Id;
            product.UpdateDate = AppDate.DateTimeNow;

            _applicationDbContext.Update(product);
            _applicationDbContext.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO product)
        {
            var user = await _authenticationService
                .GetUser();

            var hasPermission = await _productRepository
                .HasPermission(new List<int>() { product.Id }, user.Id, user.GroupId);

            if (!hasPermission)
                return Forbid();

            var exist = _applicationDbContext
                .Product
                .Where(x => x.Id != product.Id && x.Name.ToLower().Contains(product.Name.ToLower()) && x.GroupId == user.GroupId && x.Active == true)
                .Any();

            if (exist)
                return Conflict();

            var oldProduct = _applicationDbContext
                .Product
                .Where(x => x.Id == product.Id && x.Active == true)
                .First();

            oldProduct.Active = false;
            oldProduct.UpdatedByUserId = user.Id;
            oldProduct.UpdateDate = AppDate.DateTimeNow;

            _applicationDbContext.Update(oldProduct);
            _applicationDbContext.SaveChanges();

            var newProduct = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                NetPrice = product.NetPrice,
                Quanity = product.Quanity,
                Unit = product.Unit,
                PredecessorId = product.Id,
                GroupId = user.GroupId,
                CreateDate = AppDate.DateTimeNow,
                CreatedByUserId = user.Id,
                Active = true
            };

            await _applicationDbContext
                .Product
                .AddAsync(newProduct);

            await _applicationDbContext
                .SaveChangesAsync();

            return Ok(newProduct.Id);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        [Route("GetProductsForRent")]
        public async Task<IActionResult> GetProductsForRent()
        {
            var products = await _productRepository
                .GetUserProducts();

            var rentProducts = await _productRepository
                .GetRentProducts();

            if (rentProducts is null)
                return Ok(products);

            foreach (var item in rentProducts)
            {
                var product = products
                    .Where(x => x.Id == item.ProductId)
                    .FirstOrDefault();

                if (product is null)
                {
                    var predecessorProduct = _applicationDbContext
                        .Product
                        .Where(x => x.PredecessorId == item.ProductId)
                        .FirstOrDefault();

                    if (predecessorProduct is null)
                        continue;

                    product = products
                        .Where(x => x.Id == predecessorProduct.Id)
                        .FirstOrDefault();

                    if (product == null)
                        continue;
                }

                if (product is not null)
                {
                    product.Quanity -= item.Quanity;
                }

                if (product.Quanity <= 0)
                {
                    products.Remove(product);
                }
            }

            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Employee")]
        [Route("Rent")]
        public async Task<IActionResult> Rent(RentProductDTO rentProduct)
        {
            var user = await _authenticationService
                 .GetUser();

            var productIds = rentProduct
                .Products
                .Select(s => s.Id)
                .ToList();

            var hasPermission = await _productRepository
                .HasPermission(productIds, user.Id, user.GroupId);

            if (!hasPermission)
                return Forbid();

            var customerId = 0;

            var customerIsNullOrEmpty = IsCustomerNullOrEmpty(rentProduct.Customer);

            if (!customerIsNullOrEmpty)
            {
                var customer = new Customer
                {
                    NameSurname = rentProduct.Customer.NameSurname,
                    PhoneNumber = rentProduct.Customer.PhoneNumber,
                    Email = rentProduct.Customer.Email,
                    Street = rentProduct.Customer.Street,
                    ZipCode = rentProduct.Customer.ZipCode,
                    City = rentProduct.Customer.City,
                    GroupId = user.GroupId,
                    CreateDate = AppDate.DateTimeNow,
                    CreatedByUserId = user.Id,
                    Active = true
                };

                await _applicationDbContext
                    .Customer
                    .AddAsync(customer);

                await _applicationDbContext
                   .SaveChangesAsync();

                customerId = customer.Id;
            }

            var numberRentOfDay = _productRepository
                .CreateNumberRentOfDay(user.GroupId);

            var rent = new Rent
            {
                NumberRentOfDay = numberRentOfDay,
                Comments = rentProduct.Comments,
                GroupId = user.GroupId,
                CustomerId = customerId,
                CreateDate = AppDate.DateTimeNow,
                CreatedByUserId = user.Id,
                Active = true
            };

            await _applicationDbContext
                .Rent
                .AddAsync(rent);

            await _applicationDbContext
                .SaveChangesAsync();

            var rentProducts = new List<RentProduct>();

            foreach (var product in rentProduct.Products)
            {
                var rp = new RentProduct
                {
                    Quanity = product.Quanity,
                    ProductId = product.Id,
                    RentId = rent.Id,
                    CreateDate = AppDate.DateTimeNow,
                    CreatedByUserId = user.Id,
                    Active = true
                };

                rentProducts.Add(rp);
            }

            await _applicationDbContext
                .RentProduct
                .AddRangeAsync(rentProducts);

            await _applicationDbContext
                .SaveChangesAsync();

            return Ok(numberRentOfDay);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        [Route("GetActiveRent")]
        public async Task<IActionResult> GetActiveRent()
        {
            var user = await _authenticationService
                .GetUser();

            var rent = _applicationDbContext
                .Rent
                .Where(x => x.GroupId == user.GroupId && x.Active == true)
                .ToList();

            var result = _mapper
                .Map<List<RentDTO>>(rent)
                .OrderByDescending(x => x.CreateDate);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        [Route("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _authenticationService
                .GetUser();

            var rent = _applicationDbContext
                .Rent
                .Where(x => x.Id == id && x.GroupId == user.GroupId)
                .FirstOrDefault();

            if (rent == null)
                return Forbid();

            var products = _productRepository
               .GetProductsByRentId(id);

            var customer = _applicationDbContext
                .Customer
                .Where(x => x.Id == rent.CustomerId)
                .FirstOrDefault();

            var customerDTO = _mapper
                .Map<CustomerDTO>(customer);

            return Ok(new RentDetiailsDTO()
            {
                Product = products,
                Customer = customerDTO
            });
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        [Route("GetFinishedDetails")]
        public async Task<IActionResult> GetFinishedDetails(int id)
        {
            var user = await _authenticationService
                .GetUser();

            var rent = _applicationDbContext
                .Rent
                .Where(x => x.Id == id && x.GroupId == user.GroupId)
                .FirstOrDefault();

            if (rent == null)
                return Forbid();

            var products = _productRepository
               .GetProductsByRentId(id);

            var customer = _applicationDbContext
                .Customer
                .Where(x => x.Id == rent.CustomerId)
                .FirstOrDefault();

            var customerDTO = _mapper
                .Map<CustomerDTO>(customer);

            var netPrice = _applicationDbContext
                .RentFinished
                .Where(x => x.RentId == id)
                .Select(s => s.NetPrice)
                .FirstOrDefault();

            return Ok(new RentDetiailsDTO()
            {
                Product = products,
                Customer = customerDTO,
                NetPrice = Math.Round(netPrice, 2)
            });
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Employee")]
        [Route("FinishRent")]
        public async Task<IActionResult> FinishRent(int id)
        {
            var dateTimeFinish = AppDate.DateTimeNow;

            var user = await _authenticationService
                .GetUser();

            var rent = _applicationDbContext
                .Rent
                .Where(x => x.Id == id && x.GroupId == user.GroupId)
                .FirstOrDefault();

            if (rent == null)
                return Forbid();

            rent.Active = false;
            rent.UpdateDate = AppDate.DateTimeNow;
            rent.UpdatedByUserId = user.Id;

            _applicationDbContext
               .Rent
               .Update(rent);

            await _applicationDbContext
                .SaveChangesAsync();

            var elapsedTime = dateTimeFinish - rent.CreateDate;

            var products = _productRepository
                .GetProductsByRentId(id);

            decimal netPrice = 0;

            foreach (var product in products)
            {
                netPrice += ComputeRentNetPrice(elapsedTime, product.Unit, product.NetPrice) * product.Quanity;
            }

            decimal roundNetPrice = Math.Round(netPrice, 2);

            var rentFinished = new RentFinished
            {
                GroupId = user.GroupId,
                RentId = rent.Id,
                NumberRentOfDay = rent.NumberRentOfDay,
                ElapsedTime = elapsedTime,
                NetPrice = roundNetPrice,
                CreateDate = AppDate.DateTimeNow,
                CreatedByUserId = user.Id,
                Active = true
            };

            _applicationDbContext
               .RentFinished
               .Update(rentFinished);

            await _applicationDbContext
                .SaveChangesAsync();

            return Ok(new
            {
                ElapsedTime = elapsedTime.Duration().ToString(@"dd\:hh\:mm\:ss"),
                NetPrice = roundNetPrice
            });
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        [Route("GetRentFinished")]
        public async Task<IActionResult> GetRentFinished(string dateFrom, string dateTo)
        {
            var dateTimeFrom = DateTime.ParseExact(dateFrom, "dd.MM.yyyy", null).ToUniversalTime();
            var dateTimeTo = DateTime.ParseExact(dateTo, "dd.MM.yyyy", null).ToUniversalTime()
                .AddHours(23)
                .AddMinutes(59)
                .AddSeconds(59);

            var user = await _authenticationService
                .GetUser();

            var result = (from rf in _applicationDbContext.RentFinished.AsQueryable()
                          join r in _applicationDbContext.Rent.AsQueryable() on rf.RentId equals r.Id
                          where rf.GroupId == user.GroupId && r.Active == false && rf.CreateDate >= dateTimeFrom && rf.CreateDate <= dateTimeTo
                          select new RentDTO
                          {
                              Id = r.Id,
                              NumberRentOfDay = rf.NumberRentOfDay,
                              Comments = r.Comments,
                              CustomerId = r.CustomerId,
                              ElapsedTime = rf.ElapsedTime.Duration().ToString(@"dd\:hh\:mm\:ss"),
                              CreateDate = rf.CreateDate
                          })
                          .OrderByDescending(x => x.CreateDate)
                          .ToList();

            return Ok(result);
        }

        private static bool IsCustomerNullOrEmpty(CustomerDTO customer)
        {
            if (string.IsNullOrEmpty(customer.NameSurname) &&
                string.IsNullOrEmpty(customer.PhoneNumber) &&
                string.IsNullOrEmpty(customer.Email) &&
                string.IsNullOrEmpty(customer.Street) &&
                string.IsNullOrEmpty(customer.ZipCode) &&
                string.IsNullOrEmpty(customer.City))
            {
                return true;
            }
               
            return false;
        }

        private static decimal ComputeRentNetPrice(TimeSpan elapsedTime, Unit unit, decimal netPrice)
        {
            if (unit == Unit.Minute)
            {
                return (decimal) elapsedTime.TotalMinutes * netPrice;
            }

            if (unit == Unit.Hour)
            {
                return (decimal) elapsedTime.TotalHours * netPrice;
            }

            return (decimal) elapsedTime.TotalDays * netPrice;
        }
    }
}
