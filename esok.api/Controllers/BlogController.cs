using AutoMapper;
using esok.api.Data;
using esok.api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace esok.api.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

        public BlogController(ApplicationDbContext applicationDbContext,
            IMemoryCache memoryCache,
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _memoryCache = memoryCache;
            _mapper = mapper;
            _memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(6));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Articles")]
        public async Task<IActionResult> Articles()
        {
            var cachedArticles = new List<ShortArticleDTO>();

            if (!_memoryCache.TryGetValue("articles", out cachedArticles))
            {
                var artilces = await _applicationDbContext
                    .Article
                    .Where(x => x.Active == true)
                    .ToListAsync();

                var result = _mapper
                    .Map<List<ShortArticleDTO>>(artilces)
                    .OrderByDescending(x => x.CreateDate)
                    .ToList();

                _memoryCache
                    .Set("articles", result, _memoryCacheEntryOptions);

                return Ok(result);
            }

            return Ok(cachedArticles);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Article")]
        public async Task<IActionResult> Article(string title)
        {
            if (string.IsNullOrEmpty(title))
                return NotFound();

            var cachedArticle = new ArticleDTO();

            if (!_memoryCache.TryGetValue(title, out cachedArticle))
            {
                var artilce = await _applicationDbContext
                    .Article
                    .Where(x => x.Address.Contains(title) && x.Active == true)
                    .FirstOrDefaultAsync();

                var result = _mapper
                    .Map<ArticleDTO>(artilce);

                if (result == null)
                    return NotFound();

                _memoryCache
                    .Set(title, result, _memoryCacheEntryOptions);

                return Ok(result);
            }

            return Ok(cachedArticle);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("LatestEntries")]
        public async Task<IActionResult> LatestEntries()
        {
            var cachedArticles = new List<ShortArticleDTO>();

            if (!_memoryCache.TryGetValue("latestentries", out cachedArticles))
            {
                var artilces = await _applicationDbContext
                    .Article
                    .Where(x => x.Active == true)
                    .OrderByDescending(x => x.CreateDate)
                    .Take(5)
                    .ToListAsync();

                var result = _mapper
                    .Map<List<ShortArticleDTO>>(artilces)
                    .ToList();

                _memoryCache
                    .Set("latestentries", result, _memoryCacheEntryOptions);

                return Ok(result);
            }

            return Ok(cachedArticles);
        }
    }
}
