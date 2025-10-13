using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;
class TcpChatServer
{
    // ConcurrentDictionary - потокобезпечна колекція для зберігання всіх клієнтів
    // Дозволяє безпечно додавати/видаляти клієнтів із кількох потоків одночасно
    // Ключ — унікальний ID клієнта, значення — TcpClient
    static ConcurrentDictionary<string, TcpClient> clients = new();

    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Сервер запущено. Очікування клієнтів...");

        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            string id = Guid.NewGuid().ToString(); // унікальний ID клієнта
            clients.TryAdd(id, client);
            Console.WriteLine($"[{id}] Новий клієнт підключився.");

            _ = Task.Run(() => HandleClientAsync(id, client)); //
        }
    }

    static async Task HandleClientAsync(string id, TcpClient client)
    {
        try
        {
            using var reader = new StreamReader(client.GetStream(), Encoding.UTF8);
            using var writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };
            // AutoFlush = true — автоматично відправляє дані після кожного WriteLine(), не чекаючи заповнення буфера

            while (true)
            {
                string? message = await reader.ReadLineAsync(); // читаємо асинхронно з потоку
                if (message == null) break; // якщо прочитали null, клієнт відключився

                Console.WriteLine($"[{id}] {message}");

                // Розсилка всім іншим клієнтам
                await BroadcastAsync($"{id}: {message}", id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка у клієнта [{id}]: {ex.Message}");
        }
        finally
        {
            clients.TryRemove(id, out _);
            client.Close();
            Console.WriteLine($"[{id}] Клієнт відключився.");
        }
    }

    // метод надсилання повідомлення усім клієнтам, окрім вілправника (клієнти у колекції clients)
    static async Task BroadcastAsync(string message, string senderId)
    {
        foreach (var pair in clients) 
        {
            if (pair.Key != senderId) // щоб не відправляти повідомлення самому собі
            {
                try
                {
                    var writer = new StreamWriter(pair.Value.GetStream(), Encoding.UTF8) { AutoFlush = true };
                    await writer.WriteLineAsync(message); // відправляємо рядок з переносом (\n)
                }
                catch { }
            }
        }
    }
}
