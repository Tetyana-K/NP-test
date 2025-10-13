using System.Net.Sockets;
using System.Text;

class AsyncClient
{
    static async Task Main()
    {
        string serverIp = "127.0.0.1"; // IP сервера
        int port = 5001;               // порт сервера

        Console.WriteLine($"Введіть ім'я:");
        string name = Console.ReadLine() ?? "";

        using TcpClient client = new TcpClient();
        Console.WriteLine($"Підключаємось до сервера {serverIp}:{port} ...");
        await client.ConnectAsync(serverIp, port); // асинхронне підключення до сервера

        using NetworkStream stream = client.GetStream(); // отримуємо потік для обміну даними

        // формуємо повідомлення серверу
        string message = $"Привіт від {name}, асинхронний сервере!";
        byte[] data = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(data, 0, data.Length); //асинхронно пишемо дані (повідомлення серверу) в потік
        Console.WriteLine($"[{DateTime.Now}] Надіслано: {message}");

        // читаємо відповідь асинхронно
        byte[] buffer = new byte[1024];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        string reply = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine($"[{DateTime.Now}] Відповідь сервера: {reply}");

        Console.WriteLine("Натисни Enter, щоб вийти...");
        Console.ReadLine();
    }
}
