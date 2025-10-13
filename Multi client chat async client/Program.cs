using System.Net.Sockets;
using System.Text;

class AsyncChatClient
{
    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string serverIp = "127.0.0.1"; // IP сервера
        int port = 5000;               // порт сервера

        using TcpClient client = new TcpClient();
        Console.WriteLine($"Підключаємося до сервера {serverIp}:{port}...");
        await client.ConnectAsync(serverIp, port);
        Console.WriteLine("Підключено!");

        using var reader = new StreamReader(client.GetStream(), Encoding.UTF8);
        using var writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };

        // Запускаємо окремий Task для читання повідомлень від сервера
        _ = Task.Run(async () =>
        {
            try
            {
                while (true)
                {
                    string serverMessage = await reader.ReadLineAsync();
                    if (serverMessage == null) break; // сервер закрив з'єднання
                    Console.WriteLine(serverMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка читання: {ex.Message}");
            }
        });

        // Основний цикл введення користувача
        while (true)
        {
            string message = Console.ReadLine() ?? "";
            if (message.ToLower() == "exit") break;

            try
            {
                await writer.WriteLineAsync(message); // надсилаємо серверу
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка відправки: {ex.Message}");
                break;
            }
        }

        Console.WriteLine("Вихід із чату...");
        client.Close();
    }
}

