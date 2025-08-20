using AutoMapper;
using esok.api.Data;
using esok.api.DTO;
using esok.api.Interfaces.Repository;
using esok.api.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace esok.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _applicationDbContext;

        public ReportController(IAuthenticationService authenticationService,
            IProductRepository productRepository,
            IMapper mapper,
            ApplicationDbContext applicationDbContext)
        {
            _authenticationService = authenticationService;
            _productRepository = productRepository;
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [Route("LastTwoWeeksStats")]
        public async Task<IActionResult> LastTwoWeeksStats()
        {
            var dateTimeNow = DateTime.Now;
            
            var dateNowUTC = DateTime.UtcNow;

            var offset = dateTimeNow.Subtract(dateNowUTC).TotalMinutes;

            var today = dateTimeNow.Date.ToUniversalTime();
            
            var lastWeekDates = new List<DateTime>();
            
            for (int i = 6; i >= 0; i--)
            {
                var day = today.AddDays(-(int)today.DayOfWeek - i);

                if (i == 0)
                    day = day.AddSeconds(3599);

                lastWeekDates.Add(day);
            }

            var thisWeekDates = new List<DateTime>();

            for (int i = 1; i <= 7; i++)
            {
                var day = today.AddDays(-(int)today.DayOfWeek + i);

                if (i == 7)
                   day = day.AddSeconds(3599);

                thisWeekDates.Add(day);
            }

            var lastRent = await _productRepository
                .GetAllRent(lastWeekDates.First(), thisWeekDates.Last());

            lastRent.ForEach(x => x.CreateDate = x.CreateDate.AddMinutes(offset));

            var groupedLastRentProducts = lastRent
                .GroupBy(x => x.CreateDate.Date)
                .Select(s => new { s.Key.Date, Total = s.Count() })
                .ToList();

            var lastWeek = new List<int>();

            foreach (var day in lastWeekDates)
            {
                var rentsInDay = groupedLastRentProducts
                    .Where(x => x.Date == day.Date)
                    .FirstOrDefault();

                if (rentsInDay == null)
                {
                    lastWeek.Add(0);
                    continue;
                }

                lastWeek.Add(rentsInDay.Total);
            }

            var thisWeek = new List<int>();

            foreach (var day in thisWeekDates)
            {
                var rentsInDay = groupedLastRentProducts
                    .Where(x => x.Date == day.Date)
                    .FirstOrDefault();

                if (rentsInDay == null)
                {
                    thisWeek.Add(0);
                    continue;
                }

                thisWeek.Add(rentsInDay.Total);
            }

            return Ok(
                new
                {
                    ThisWeek = thisWeek,
                    LastWeek = lastWeek
                });
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [Route("LastYearRevenueStats")]
        public async Task<IActionResult> LastYearRevenueStats()
        {
            var user = await _authenticationService
                .GetUser();

            var dateNow = DateTime.Now.Date;

            var dateTimeNow = new DateTime(dateNow.Year, 1, 1, dateNow.Hour, 0, 0);

            var dateTimeFrom = dateTimeNow.ToUniversalTime();
            var dateTimeTo = dateTimeFrom.AddYears(1);

            var lastLastYearRevenue = await _applicationDbContext
                .RentFinished
                .Where(x => x.GroupId == user.GroupId && x.CreateDate >= dateTimeFrom && x.CreateDate < dateTimeTo)
                .ToListAsync();

            var groupedLastLastYearRevenue = lastLastYearRevenue
                .GroupBy(x => x.CreateDate.Date.Month)
                .Select(s => new { Month = s.Key, Total = s.Select(f => f.NetPrice).Sum() })
                .ToList();

            var result = new List<decimal>();

            for (int i = 1; i <= dateNow.Month; i++)
            {
                var revenue = groupedLastLastYearRevenue
                    .Where(x => x.Month == i)
                    .FirstOrDefault();

                if (revenue == null)
                {
                    result.Add(0);
                    continue;
                }
                   
                if (revenue.Month == i)
                    result.Add(revenue.Total);
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository
                .GetUserProducts();

            var reportProducts = _mapper
                .Map<List<ReportProductDTO>>(products)
                .ToList();

            reportProducts.ForEach(x => x.Total = x.Quanity);

            var rentProducts = await _productRepository
                .GetRentProducts();

            if (rentProducts is null)
                return Ok(products);

            foreach (var item in rentProducts)
            {
                var product = reportProducts
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

                    product = reportProducts
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

            return Ok(reportProducts);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [Route("RentFinished")]
        public async Task<IActionResult> RentFinished()
        {
            var user = await _authenticationService
                .GetUser();

            var dateNow = DateTime.Now.Date;

            var dateTimeNow = new DateTime(dateNow.Year, dateNow.Month, 1, dateNow.Hour, 0, 0);

            var dateTimeFrom = dateTimeNow.ToUniversalTime();
            var dateTimeTo = dateTimeFrom.AddMonths(1);
                
            var rentsFinished = await _applicationDbContext
                .RentFinished
                .Where(x => x.GroupId == user.GroupId && x.CreateDate >= dateTimeFrom && x.CreateDate < dateTimeTo)
                .ToListAsync();

            var netPrice = rentsFinished
                .Select(s => s.NetPrice)
                .Sum();

            var activeRents = await _applicationDbContext
                .Rent
                .Where(x => x.GroupId == user.GroupId && x.Active == true)
                .ToListAsync();

            return Ok(
                new 
                {
                    ActiveRentsCount = activeRents.Count,
                    RentsFinishedCount = rentsFinished.Count,
                    FinishedNetPrice = netPrice
                });
        }
    }
}
