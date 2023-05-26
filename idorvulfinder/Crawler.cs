using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class WebCrawlerInstance
    {
        private readonly HttpClient httpClient;
        private readonly HashSet<string> visitedUrls;

        public WebCrawlerInstance()
        {
            httpClient = new HttpClient();
            visitedUrls = new HashSet<string>();
        }

        public async Task CrawlAsync(string startUrl, int maxDepth)
        {
            await CrawlUrl(startUrl, 0, maxDepth);
        }

        private async Task CrawlUrl(string url, int depth, int maxDepth)
        {
            if (depth > maxDepth || visitedUrls.Contains(url))
                return;

            visitedUrls.Add(url);

            Console.WriteLine($"Crawling: {url}");

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var htmlContent = await response.Content.ReadAsStringAsync();

                // Extract relevant information from the HTML content
                // e.g., parse links, scrape data, etc.

                // Example: Print the title of the page
                var pageTitle = ExtractPageTitle(htmlContent);
                Console.WriteLine($"Title: {pageTitle}");

                // Example: Extract and crawl all the links on the page
                var pageLinks = ExtractPageLinks(htmlContent);
                foreach (var link in pageLinks)
                {
                    await CrawlUrl(link, depth + 1, maxDepth);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error crawling URL: {url}");
                Console.WriteLine(ex.Message);
            }
        }

        private string ExtractPageTitle(string htmlContent)
        {
            // Extract the page title from the HTML content
            // You can use a library like HtmlAgilityPack for more advanced HTML parsing

            // Example: Regex pattern to extract the page title from an HTML <title> tag
            var pattern = "<title>(.*?)</title>";
            var match = System.Text.RegularExpressions.Regex.Match(htmlContent, pattern);
            return match.Success ? match.Groups[1].Value : "N/A";
        }

        private IEnumerable<string> ExtractPageLinks(string htmlContent)
        {
            // Extract the links from the HTML content
            // You can use a library like HtmlAgilityPack for more advanced HTML parsing

            // Example: Regex pattern to extract links from <a> tags
            var pattern = "<a\\s+(?:[^>]*?\\s+)?href=\"([^\"]*)\"";
            var matches = System.Text.RegularExpressions.Regex.Matches(htmlContent, pattern);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                yield return match.Groups[1].Value;
            }
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var controller = new WebCrawlerInstance();
            await controller.CrawlAsync("https://example.com", 2);
        }
    }
}
