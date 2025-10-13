using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class AsyncUdpServer
{
    static async Task Main()
    {
        // Створюємо UDP-сервер, який слухає порт 5000
        UdpClient server = new UdpClient(5000);

        Console.WriteLine($"UDP сервер запущено (порт {(server.Client.LocalEndPoint as IPEndPoint)?.Port}). Очікування повідомлень...");

        while (true)
        {
            // Асинхронно отримуємо дані (не блокує виконання)
            UdpReceiveResult result = await server.ReceiveAsync();

            // Запускаємо обробку повідомлення в окремому завданні (Task)
            _ = Task.Run(async () =>
            {
                try
                {
                    // Розкодовуємо текст повідомлення
                    string message = Encoding.UTF8.GetString(result.Buffer); // result.Buffer містить повідомлення від клієнта (масив байтів)
                    Console.WriteLine($"[Клієнт {result.RemoteEndPoint}] {message}");

                    // Готуємо відповідь
                    string response = $"Сервер отримав: '{message}' час {DateTime.Now:T}";
                    byte[] responseData = Encoding.UTF8.GetBytes(response);

                    // Надсилаємо відповідь назад клієнту
                    await server.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка під час обробки клієнта: {ex.Message}");
                }
            });
        }
    }
}
