using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        //using HttpClient client = new HttpClient();
        ////string result = await client.GetStringAsync("https://example.com");
        //string result = await client.GetStringAsync("https://www.w3schools.com/xml/simplexsl.xml");
        //Console.WriteLine(result);
        using var client = new HttpClient();
        var response = await client.GetAsync("https://www.google.com");

        Console.WriteLine($"Протокол: {response.Version}");
    }
    
}
