using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IDatabase _redisDb;
    //внедряем логгер и подключение к Redis
    public IndexModel(ILogger<IndexModel> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redisDb = redis.GetDatabase();
    }
    //обработчик POST запроса от формы
    public IActionResult OnPost(string text)
    {
        _logger.LogDebug(text);

        if (string.IsNullOrWhiteSpace(text))
        {
            return Redirect("/index");
        }

        //создаём уникальный id
        string id = Guid.NewGuid().ToString();     

        string rankKey = "RANK-" + id;
        double rank = CalculateRank(text);
        SaveGrade(rankKey, rank);

        string similarityKey = "SIMILARITY-" + id;
        double similarity = CalculateSimilarity(text, similarityKey);
        SaveGrade(similarityKey, similarity);

        string textKey = "TEXT-" + id;
        SaveText(textKey, text);
        //перенаправляем на страницу summary и передаём id
        return Redirect($"summary?id={id}");
    }

    private void SaveText(string key, string text)
    {
        _redisDb.StringSet(key, text);
    }

    private double CalculateRank(string text)
    {
        int lettersCount = text.Count(char.IsLetter);
        int totalChars = text.Length;
        return totalChars == 0 ? 0 : (double)(totalChars - lettersCount) / totalChars;
    }
    
    private double CalculateSimilarity(string text, string currentId)
    {
        //получаем все ключи, начинающие с TEXT
        var keys = _redisDb.Multiplexer.GetServer(_redisDb.Multiplexer.GetEndPoints().First()).Keys(pattern: "TEXT-*");

        foreach (var key in keys)
        {
            //пропускаем пользовательский текст
            if (key.ToString().EndsWith(currentId))
                continue;
            
            string existingText = _redisDb.StringGet(key);
            if (existingText == text)
            {
                return 1.0;
            }
        }

        return 0.0;
    }

    private void SaveGrade(string key, double grade)
    {
        _redisDb.StringSet(key, grade.ToString());
    }
}