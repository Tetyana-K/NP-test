using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class AsyncQuoteServer
{
    static List<string> quotes = new List<string>
    {
        "Якщо спочатку у вас не виходить, спробуйте ще раз. - Вільям Едвард Хіксон",
        "A людина, яка ніколи не помилялася, ніколи не пробувала нічого нового - Альберт Ейнштейн",
        "Якби життя було передбачуваним, воно перестало б бути життям і залишилося б без смаку» - Елеонора Рузвельт",
        "Сміливість – це знати страх, але діяти всупереч йому.",
        "Неважливо, як повільно ти йдеш, поки ти не зупиняєшся. - Конфуцій"
    };

    static async Task Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        Console.WriteLine("Сервер запущено. Очікування підключень...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client); // запустити без очікування
        }
    }

    static async Task HandleClientAsync(TcpClient client)
    {
        string? clientEndPoint = client?.Client?.RemoteEndPoint?.ToString();
        DateTime connectTime = DateTime.Now;
        Console.WriteLine($"[Підключено] {clientEndPoint} о {connectTime}");

        NetworkStream? stream = client?.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // клієнт відключився

                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                if (request.ToLower() == "exit") break;

                // випадкова цитата
                Random rnd = new Random();
                string quote = quotes[rnd.Next(quotes.Count)];
                byte[] response = Encoding.UTF8.GetBytes(quote + "\n");
                await stream.WriteAsync(response, 0, response.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Помилка] {clientEndPoint}: {ex.Message}");
        }
        finally
        {
            DateTime disconnectTime = DateTime.Now;
            Console.WriteLine($"[Відключено] {clientEndPoint}, час {disconnectTime}");
            client?.Close();
        }
    }
}
