using esok.api.Application.Common;
using esok.api.Data;
using esok.api.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using Quartz;

namespace esok.api.Executors
{
    public class AddFacebookPost : IJob
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IFacebookService _facebookService;
        private readonly IMemoryCache _memoryCache;

        public AddFacebookPost(ApplicationDbContext applicationDbContext,
            IFacebookService facebookService,
            IMemoryCache memoryCache)
        {
            _applicationDbContext = applicationDbContext;
            _facebookService = facebookService;
            _memoryCache = memoryCache;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var article = await _applicationDbContext
                .Article
                .Where(x => x.Active == false)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();

            if (article == null)
                return;

            var pagePostId = await _facebookService
                .AddPost(article.PostContent, article.Address);

            if (!string.IsNullOrEmpty(pagePostId))
            {
                article.Active = true;
                article.PagePostId = pagePostId;
                article.CreateDate = AppDate.DateTimeNow;

                await _applicationDbContext
                    .SaveChangesAsync();

                _memoryCache.Remove("articles");
                _memoryCache.Remove("latestentries");
                _memoryCache.Remove(article.Title);
            }
        }

    }
}