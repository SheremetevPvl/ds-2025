using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;

namespace Valuator.Pages;
public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    private readonly IDatabase _redis;

    public SummaryModel(ILogger<SummaryModel> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis.GetDatabase();
    }

    public double Rank { get; set; }
    public double Similarity { get; set; }
    //обработка GET запросов при переходе на Summary
    public void OnGet(string id)
    {
        _logger.LogDebug(id);

        // Формирование ключей
        string rankKey = "RANK-" + id;
        string similarityKey = "SIMILARITY-" + id;

        // Получение значений из Redis
        var rankValue = _redis.StringGet(rankKey);
        var similarityValue = _redis.StringGet(similarityKey);

        if (rankValue.HasValue && double.TryParse(rankValue, out double rank))
        {
            Rank = rank;
        }
        else
        {
            _logger.LogWarning("Rank not found or invalid for ID: {Id}", id);
        }

        if (similarityValue.HasValue && double.TryParse(similarityValue, out double similarity))
        {
            Similarity = similarity;
        }
        else
        {
            _logger.LogWarning("Similarity not found or invalid for ID: {Id}", id);
        }

        _logger.LogDebug("Rank: {Rank}, Similarity: {Similarity} for ID: {Id}", Rank, Similarity, id);
    }
}
