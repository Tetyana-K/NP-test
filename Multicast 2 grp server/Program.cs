using System.Net.Sockets;
using System.Text;

// Record для зберігання інформації про Multicast-групу
// Type - тип повідомлення (логічний)
// Ip   - IP-адреса Multicast-групи
// Port - порт, на якому група слухає пакети
public record MulticastGroup(string Type, string Ip, int Port);

class MulticastServer
{
    static void Main()
    {
        // Створюємо масив Multicast-груп для різних типів повідомлень
        var groups = new[]
        {
            new MulticastGroup("news", "224.1.1.1", 5000),
            new MulticastGroup("alert", "224.1.1.2", 5001),
            new MulticastGroup("weather", "224.1.1.3", 5002)
        };

        UdpClient udpClient = new UdpClient(); // UDP-клієнт для відправки пакетів
        Console.OutputEncoding = Encoding.UTF8; 
        Console.WriteLine("Multicast сервер запущено.\n");

        int counter = 1;
        while (true)
        {
            foreach (var group in groups)
            {
                // Формуємо повідомлення з типом
                string message = $"{group.Type, 10}|Повідомлення #{counter} для {group.Type}";
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Відправляємо пакет на IP і порт групи
                udpClient.Send(data, data.Length, group.Ip, group.Port);
                Console.WriteLine($"[Сервер] Відправлено: {message} to {group.Ip}:{group.Port}");
            }

            counter++;
            Thread.Sleep(3000); // Відправка кожні 3 секунди
            Console.WriteLine();
        }
    }
}
