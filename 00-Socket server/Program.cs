using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class SimpleServer
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // 1️ Створюємо "кінцеву точку" (endpoint):
        // IPAddress.Loopback означає 127.0.0.1 (локальний комп’ютер)  house
        // 5000 — це порт, на якому сервер буде слухати підключення  flat
        // house + flat = Socket
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 5000);

        // 2️ Створюємо сокет:
        // AddressFamily.InterNetwork — IPv4
        // SocketType.Stream — потоковий сокет (для TCP)
        // ProtocolType.Tcp — використовуємо TCP протокол
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 3️ Прив’язуємо сокет до вказаного IP і порту
        server.Bind(endPoint);

        // 4️ Починаємо "слухати" порт — сервер готовий приймати підключення
        // (1 означає, що одночасно може чекати 1 клієнт у черзі)
        server.Listen(1);
        Console.WriteLine("Сервер слухає на порту 5000...");

        // 5️ Очікуємо підключення клієнта (блокуючий виклик)
        Socket client = server.Accept();
        Console.WriteLine("Клієнт підключився!");

        // 6️ Створюємо буфер для отримання даних (1024 байти)
        byte[] buffer = new byte[1024];

        // 7️ Отримуємо дані від клієнта
        int bytes = client.Receive(buffer); // bytes = скільки фактично прийшло байтів, самі дані (байти) завантажилися у buffer

        // 8️ Перетворюємо байти у рядок і виводимо
        string message = Encoding.UTF8.GetString(buffer, 0, bytes);
        Console.WriteLine("Отримано повідомлення: " + message);

        // 9️ Закриваємо підключення з клієнтом
        client.Close();

        // 10 Закриваємо сервер (звільняємо порт)
        server.Close();

        Console.WriteLine("Сервер завершив роботу.");
    }
}
