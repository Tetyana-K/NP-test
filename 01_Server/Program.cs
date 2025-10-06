using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // TcpListener = клас для створення TCP-сервера
        TcpListener listener = new TcpListener(IPAddress.Any, 5000); // створюємо сервер на порту 5000, який слухає всі IP-адреси
        listener.Start(); // запускаємо сервер
        Console.WriteLine("Сервер запущено. Очікуємо клієнта...");

        TcpClient client = listener.AcceptTcpClient(); // очікуємо підключення клієнта

        IPEndPoint? clientEndPoint = client?.Client.RemoteEndPoint as IPEndPoint; // отримуємо IP-адресу і порт клієнта
        // IPEndPoint - клас, який представляє кінцеву точку мережевого з'єднання (IP-адреса + порт)

        Console.WriteLine($"Клієнт підключився з IP: {clientEndPoint?.Address}, порт: {clientEndPoint?.Port}");


        NetworkStream stream = client.GetStream(); // отримуємо потік для читання/запису даних від клієнта

        byte[] buffer = new byte[1024]; // буфер для зберігання отриманих даних
        int bytesRead = stream.Read(buffer, 0, buffer.Length); // читаємо дані з потоку
        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead); // конвертуємо масив прочитаних байтів в рядок у кодуванні UTF-8
        Console.WriteLine("Клієнт написав: " + message);

        string reply = "Привіт від сервера!"; // це буде відповіддю сервера
        byte[] replyBytes = Encoding.UTF8.GetBytes(reply); // конвертуємо рядок у масив байтів
        stream.Write(replyBytes, 0, replyBytes.Length); // відправляємо відповідь (цей масив байтів) клієнту

        client.Close(); // закриваємо з’єднання з клієнтом
        listener.Stop(); // зупиняємо сервер
        Console.WriteLine("Сервер завершив роботу");
    }
}
/* У програмі використали порт 5000
 Будь-який порт від 0 до 65535 можна використовувати.
Порти 0–1023 — системні, зарезервовані для стандартних сервісів (HTTP = 80, HTTPS = 443, FTP = 21).
Порти 1024–49151 — “зареєстровані”, теж часто використовуються відомими програмами.
Порти 49152–65535 — динамічні, тимчасові, для приватних цілей.

Ми взяли 5000, бо він вільний, і легкий для запам’ятовування.
 
 */