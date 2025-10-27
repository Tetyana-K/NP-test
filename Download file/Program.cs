
using System;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        DownloadFileByWebClient();
        await DownloadFileAsync();
    }

    private static void DownloadFileByWebClient()
    {
        //string imageUrl = "https://www.gutenberg.org/cache/epub/43/pg43.cover.medium.jpg";
        //string imageUrl = "https://media.geeksforgeeks.org/wp-content/uploads/20250705152348042640/Request-and-Response-Cycle.webp";
        string imageUrl = "https://www.gutenberg.org/files/1524/1524-0.txt";
        //string imageUrl = "https://media.geeksforgeeks.org/wp-content/uploads/20250705152348042640/Request-and-Response-Cycle.webp";
        string fileName = "../../../hamlet.txt";

        using WebClient client = new WebClient(); // obsolete in .NET 6.0 and later
        client.DownloadFile(imageUrl, fileName);

        Console.WriteLine($"Image saved as: {fileName}");
    }

    private static async Task DownloadFileAsync()
    {
        string imageUrl = "https://www.cdnetworks.com/wos/static-resource/9e836fbe17c141689830b64157c0ba9d/QUIC-PICTURE-05-1024x560.jpg?t=1740733716166";
        string fileName = "../../../quic_protocol.jpg";

        using HttpClient client = new HttpClient();
        byte[] data = await client.GetByteArrayAsync(imageUrl); // Завантаження файлу як масиву байтів

        await File.WriteAllBytesAsync(fileName, data); // Запис масиву байтів у файл
        Console.WriteLine($"Image saved as: {fileName}");
    }
}
