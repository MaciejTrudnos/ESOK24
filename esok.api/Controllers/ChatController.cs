using esok.api.Application.Common;
using esok.api.Data;
using esok.api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace esok.api.Controllers
{
    public class ChatController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMemoryCache _memoryCache;

        public ChatController(HttpClient httpClient,
            ApplicationDbContext applicationDbContext,
            IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _applicationDbContext = applicationDbContext;
            _memoryCache = memoryCache;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Answer")]
        public async Task<IActionResult> Answer([FromBody] string question)
        {
            if (string.IsNullOrEmpty(question))
                return Ok("W czym mogę pomóc?");

            var cachedAnswer = string.Empty;

            if (!_memoryCache.TryGetValue(question, out cachedAnswer))
            {
                var jsonBody = JsonSerializer
                    .Serialize(new { Question = question });

                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, "http://esok-chatbot-container:5000")
                {
                    Content = content
                };

                var response = await _httpClient
                    .SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response
                        .Content
                        .ReadAsStringAsync();

                    var result = JsonSerializer
                        .Deserialize<ChatbotAnswerDTO>(responseBody);

                    var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromDays(1));

                    var saveInCacheTask = Task.Run(() => _memoryCache
                        .Set(question, result.Answer, memoryCacheEntryOptions));

                    var saveInDbTask = SaveQuestionAnswer(question, result);

                    await Task.WhenAll(saveInCacheTask, saveInDbTask);

                    return Ok(result.Answer);
                }
     
                return StatusCode(503);
            }

            return Ok(cachedAnswer);
        }

        private async Task SaveQuestionAnswer(string question, ChatbotAnswerDTO chatbotAnswerDTO)
        {
            var savedQuestion = await _applicationDbContext
                .ChatbotQuestionAnswer
                .Where(x => x.Question.Contains(question))
                .FirstOrDefaultAsync();

            if (savedQuestion != null)
            {
                savedQuestion.CreateDate = AppDate.DateTimeNow;

                _applicationDbContext
                    .ChatbotQuestionAnswer
                    .Update(savedQuestion);
            }
            else
            {
                var chatbotQuestionAnswer = new ChatbotQuestionAnswer()
                {
                    Question = question,
                    Score = chatbotAnswerDTO.Score,
                    Start = chatbotAnswerDTO.Start,
                    End = chatbotAnswerDTO.End,
                    Answer = chatbotAnswerDTO.Answer,
                    CreateDate = AppDate.DateTimeNow,
                };

                await _applicationDbContext
                    .ChatbotQuestionAnswer
                    .AddAsync(chatbotQuestionAnswer);
            }

            await _applicationDbContext
                .SaveChangesAsync();
        }
    }
}
