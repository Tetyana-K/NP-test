using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class BroadcastChat
{
    static readonly int port = 8888;
    static readonly ConcurrentDictionary<string, DateTime> onlineUsers = new();
    static readonly object consoleLock = new(); //для безпечного виводу
    static UdpClient? udpClient;
    static string userName = "";

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Write("Введіть своє ім'я: ");
        userName = Console.ReadLine() ?? "Anonymous";

        udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
        udpClient.Client.ReceiveBufferSize = 65535;

        IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        udpClient.Client.Bind(localEP);

        //Запускаємо потік прийому повідомлень
        Task.Run(() => ReceiveMessages());

        //Періодичне відправлення "hello"
        Task.Run(() => SendHelloLoop());

        //Прибирання неактивних користувачів
        Task.Run(() => CleanupInactiveUsers());

        //Періодичне оновлення списку онлайн
        Task.Run(() => RefreshUserListLoop());

        Console.WriteLine("=== Чат запущено. Введіть повідомлення та натисніть Enter ===");

        while (true)
        {
            string text = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(text))
                continue;

            SendMessage($"{userName}: {text}");
        }
    }

    //Відправлення broadcast повідомлення
    static void SendMessage(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, port);
            udpClient?.Send(data, data.Length, broadcastEP);
        }
        catch 
        {
            //ігноруємо можливі короткі помилки 
        }
    }

    // Прийом broadcast повідомлень
    static void ReceiveMessages()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
        while (true)
        {
            try
            {
                byte[] data = udpClient!.Receive(ref remoteEP);
                string msg = Encoding.UTF8.GetString(data);

                // Якщо це "HELLO", оновлюємо онлайн-користувача
                if (msg.StartsWith("HELLO:"))
                {
                    string user = msg.Substring(6);
                    bool isNew = !onlineUsers.ContainsKey(user);
                    onlineUsers[user] = DateTime.Now;

                    if (isNew)
                        PrintOnlineUsers(); //  оновлення при появі нового
                }
                else
                {
                    lock (consoleLock)
                    {
                        Console.WriteLine($"\n{msg}");
                        Console.Write("> ");
                    }
                }
            }
            catch { }
        }
    }

    // Відправлення "HELLO" пакетів для сповіщення, що користувач онлайн
    static void SendHelloLoop()
    {
        while (true)
        {
            SendMessage($"HELLO:{userName}");
            Thread.Sleep(3000); // кожні 3 секунди
        }
    }

    // Видаляємо користувачів, які не надсилали "HELLO" 10 секунд
    static void CleanupInactiveUsers()
    {
        while (true)
        {
            var now = DateTime.Now;
            foreach (var kvp in onlineUsers)
            {
                if ((now - kvp.Value).TotalSeconds > 10)
                    onlineUsers.TryRemove(kvp.Key, out _);
            }
            Thread.Sleep(5000);
        }
    }

    // Оновлення списку користувачів у консоль раз на 8 секунд
    static void RefreshUserListLoop()
    {
        while (true)
        {
            PrintOnlineUsers();
            Thread.Sleep(8000);
        }
    }

    // Вивід списку користувачів
    static void PrintOnlineUsers()
    {
        lock (consoleLock)
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();
            Console.WriteLine("=== Онлайн користувачі ===");
            foreach (var user in onlineUsers.Keys)
                Console.WriteLine($"\t{user}");
            Console.WriteLine("==============================");
            Console.Write("> ");
        }
    }
}
