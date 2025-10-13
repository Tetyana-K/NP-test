using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public record MulticastGroup(string Type, string Ip, int Port);

class MulticastClient
{
    static void Main()
    {
        // Логічна підписка клієнта: які типи повідомлень він хоче отримувати
       List<string> subscriptions = ["news", "weather"];

        // Всі доступні Multicast-групи
        List<MulticastGroup> groups = 
        [
            new MulticastGroup("news", "224.1.1.1", 5000),
            new MulticastGroup("alert", "224.1.1.2", 5001),
            new MulticastGroup("weather", "224.1.1.3", 5002)
        ];

        // Створюємо Udp-сокети для кожної групи, на яку підписаний клієнт
        List<UdpClient> clients = new List<UdpClient>(); // поки пусто

        for (int i = 0; i < subscriptions.Count; i++)
        {
            string type = subscriptions[i];

            // Знаходимо групу, яка відповідає потрібному типу
            var group = groups.First(g => g.Type == type);

            // Створюємо UDP-сокет
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Дозволяємо повторне використання порту (ReuseAddress)
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            // Прив'язуємо сокет до порту групи
            socket.Bind(new IPEndPoint(IPAddress.Any, group.Port));

            // Обгортаємо сокет у UdpClient для зручності
            UdpClient udpClient = new UdpClient { Client = socket };

            // Приєднуємося до Multicast-групи
            udpClient.JoinMulticastGroup(IPAddress.Parse(group.Ip));

            clients.Add(udpClient); // Додаємо клієнта
            Console.WriteLine($"[Клієнт] Підписка на {group.Type} - {group.Ip}:{group.Port}");
        }

        // Основний цикл отримання повідомлень
        while (true)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                UdpClient udpClient = clients[i];
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

                // Перевіряємо, чи є пакети для отримання
                if (udpClient.Available > 0)
                {
                    byte[] data = udpClient.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(data);

                    // Виводимо повідомлення з групи
                    Console.WriteLine($"[Клієнт] Отримано: {message}");
                }
            }

            System.Threading.Thread.Sleep(100); // Невелика пауза, щоб не навантажувати CPU
        }
    }
}
