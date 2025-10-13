using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class AsyncUdpClient
{
    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        using UdpClient client = new UdpClient();

        // Адреса сервера (localhost, порт 5000)
        IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 5000);

      //  client.Connect(serverEP); // логічне підключення, не фізичне

        Console.Write("Введіть повідомлення серверу: ");
        string message = Console.ReadLine() ?? "Привіт!";

        // Перетворюємо текст у байти
        byte[] data = Encoding.UTF8.GetBytes(message);

        // Асинхронно надсилаємо дані серверу
        await client.SendAsync(data, data.Length, serverEP); // так буде помилка, якщо робили Connect()
       // await client.SendAsync(data, data.Length); //  так надсилаємо, якщо був Connect
        Console.WriteLine("[Клієнт] Повідомлення відправлено.");

        // Асинхронно чекаємо відповідь
        var result = await client.ReceiveAsync();
        string response = Encoding.UTF8.GetString(result.Buffer);

        Console.WriteLine($"[Сервер] {response}");

        client.Close();
    }
}
