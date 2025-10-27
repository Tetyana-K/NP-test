using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

class Program
{
    static async Task Main()
    {
        string searchUrl = "https://www.gutenberg.org/ebooks/search/?query=hamlet";
        using var http = new HttpClient(); 
        string html = await http.GetStringAsync(searchUrl);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Знаходимо всі посилання на книги
        var links = doc.DocumentNode
            .SelectNodes("//li[@class='booklink']//a[@class='link']")
            ?.Select(a => "https://www.gutenberg.org" + a.GetAttributeValue("href", ""))
            .ToList();

        if (links != null && links.Count > 0)
        {
            Console.WriteLine("Знайдено перші результати:");
            foreach (var link in links)
                Console.WriteLine(link);
        }
        else
        {
            Console.WriteLine("Не знайдено результатів.");
        }
    }
}
