using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NorthwindApp.Helpers;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;

namespace NorthwindApp.Middleware
{
    public class CacheMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly RequestDelegate _next;
        private readonly IRepository<Category> _categoryRepository;

        public CacheMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            IMemoryCache memoryCache,
            IRepository<Category> categoryRepository)
        {
            _next = next;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _categoryRepository = categoryRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            int categoryId = GetCategoryId(context.Request);
            if (_memoryCache.TryGetValue(categoryId, out var result))
            {
                context.Response.ContentType = Constants.ContentTypeDownload;
                await GetImageFromDirectory(context, categoryId);
            }
            else
            {
                await _next.Invoke(context);

                if (context.Response.ContentType is Constants.ContentTypeDownload)
                {
                    int cacheTime = _configuration.GetValue<int>(Constants.CacheTimeInMinutesKey);
                    AddCategoryIdToCache(categoryId, cacheTime);
                    await AddImageToDirectory(categoryId);
                }
            }
        }

        private int GetCategoryId(HttpRequest request)
        {
            string queryParemeter = request.Query["categoryId"].ToString();
            int.TryParse(queryParemeter, out int categoryId);

            if (categoryId == 0)
            {
                string lastElement = request.Path.ToString().Split('/').Last();
                int.TryParse(lastElement, out categoryId);
            }

            return categoryId;
        }

        private void AddCategoryIdToCache(int categoryId, int timeStampInMinutes)
        {
            _memoryCache.Set(
                categoryId,
                new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(timeStampInMinutes)));
        }

        private async Task GetImageFromDirectory(HttpContext context, int categoryId)
        {
            string cacheFolderPath =
                _configuration.GetValue<string>(Constants.CachedFolderNameKey);
            await using FileStream fileStream =
                new FileStream($"{cacheFolderPath}/{categoryId}.jpg", FileMode.Open);
            await fileStream.CopyToAsync(context.Response.Body);
        }

        private async Task AddImageToDirectory(int categoryId)
        {
            string cacheFolderPath = 
                _configuration.GetValue<string>(Constants.CachedFolderNameKey);
            Category category = await _categoryRepository.GetByIdAsync(categoryId);
            await using FileStream fileStream = 
                File.Create($"{cacheFolderPath}/{categoryId}.jpg");
            await fileStream.WriteAsync(
                category.Picture,
                0, 
                category.Picture.Length);
        }
    }
}
