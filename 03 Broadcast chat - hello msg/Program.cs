using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class BroadcastChat
{
    static int port = 8888;
    // колекція користувачів, які зараз онлайн (key = ім'я, value = час останнього "hello")
    static ConcurrentDictionary<string, DateTime> onlineUsers = new();
    static readonly object consoleLock = new();

    static void Main()
    {
        using (UdpClient udpClient = new UdpClient())
        {
            udpClient.Client.ReceiveBufferSize = 65535; // збільшуємо буфер прийому, зменшує ризик втрати пакетів
            udpClient.EnableBroadcast = true;

            // дозволяємо кільком клієнтам працювати на одному порту
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            Console.Write("Введіть ім'я: ");
            string name = Console.ReadLine() ?? "Noname";

            // Паралельно запускаємо три задачі -  прийом і ping ('HELLO') та очистку неактивних користувачів
            Task.Run(() => ReceiveMessages(udpClient, name));
            Task.Run(() => SendHelloLoop(udpClient, name));
            Task.Run(() => CleanupInactiveUsers());

            IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, port);
            Console.WriteLine("Ви у чаті. Пишіть повідомлення");

            while (true)
            {
                string? text = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(text)) continue;

                // формуємо пакет виду: MSG|ім`я|повідомлення
                string msg = $"MSG|{name}|{text}";
                byte[] data = Encoding.UTF8.GetBytes(msg);
                udpClient.Send(data, data.Length, broadcastEP); // відправляємо всім у локальній мережі (broadcast)
            }
        }
    }
    // прийом broadcast-повідомлень ('HELLO', 'MSG)
    static void ReceiveMessages(UdpClient udpClient, string myName)
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
        while (true)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEP);
                string msg = Encoding.UTF8.GetString(data);

                string[] parts = msg.Split('|');
                if (parts.Length < 2) continue;

                string type = parts[0]; // тип пакета: HELLO або MSG
                string sender = parts[1]; // ім'я відправника пакету

                if (sender == myName) continue; // не обробляємо свої пакети

                if (type == "HELLO")
                {
                    onlineUsers[sender] = DateTime.Now; // оновлюємо користувача у onlineUsers
                    PrintOnlineUsers();
                }
                else if (type == "MSG" && parts.Length >= 3) // обробка  повідомлення MSG
                {
                    string text = parts[2];
                    Console.WriteLine($"\n[{sender}]: {text}");
                    Console.Write("> ");
                }
            }
            catch { }
        }
    }

    // періодично (кожні 5 секунд) розсилаємо HELLO пакети
    static void SendHelloLoop(UdpClient udpClient, string name)
    {
        IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, port);
        while (true)
        {
            string msg = $"HELLO|{name}";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            udpClient.Send(data, data.Length, broadcastEP);
            Thread.Sleep(5000); // кожні 5 секунд надсилаємо ping
        }
    }

    //видалення користувачів, які не оновлювали статус > 20 секунд
    static void CleanupInactiveUsers()
    {
        while (true)
        {
            foreach (var user in onlineUsers)
            {
                if ((DateTime.Now - user.Value).TotalSeconds > 20)
                {
                    onlineUsers.TryRemove(user.Key, out _);
                    PrintOnlineUsers();
                }
            }
            Thread.Sleep(10000);
        }
    }

    // виведення списку активних користувачів чату
    static void PrintOnlineUsers()
    {
        lock (consoleLock) // блокуємо тільки консоль, не словник
        {
           // Console.Clear();
            //Console.SetCursorPosition(0, 0);
            Console.WriteLine("Онлайн користувачі:");

            foreach (var u in onlineUsers.Keys)
                Console.WriteLine($"\t{u}");
            Console.WriteLine($"\n[{DateTime.Now:T}] Онлайн: {onlineUsers.Count}");

            Console.WriteLine(new string('-', 30));

            Console.WriteLine("\nПишіть повідомлення");
            Console.Write("> ");
        }
    }
}
