using Microsoft.AspNetCore.Mvc;
using WebCrawler;

namespace idorvulfinder.Controllers;

[ApiController]
[Route("[controller]")]
public class CrawlerController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<CrawlerController> _logger;

    public CrawlerController(ILogger<CrawlerController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "RunController")]
    public async Task<string> GetAsync()
    {

        var controller = new WebCrawlerInstance();
        await controller.CrawlAsync("https://google.com", 2);
        return "210 ok";
    }
}

