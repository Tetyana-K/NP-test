using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
/*
  UDP не встановлює постійне з`єднання між клієнтом і сервером.
Кожен пакет — це окрема "листівка", яку можна відправити будь-кому.
*/
class UdpServer
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Створюємо UDP-сервер, який слухає порт 5000
        UdpClient server = new UdpClient(5000);

        // Точка призначення (endpoint) клієнта — спочатку порожня (0).
        // Вона буде заповнена автоматично при отриманні даних.
        IPEndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);

        Console.WriteLine("UDP сервер запущено. Очікування повідомлень...");

        while (true)
        {
            // Отримуємо дані (масив байтів) від будь-якого клієнта
            byte[] data = server.Receive(ref clientEP);

            Console.WriteLine($"Прийшов клієнт {clientEP.Address}:{clientEP.Port}");
            // Перетворюємо байти у текст (кодування UTF8)
            string message = Encoding.UTF8.GetString(data);

            Console.WriteLine($"[Клієнт {clientEP}] {message}");
            
            // Формуємо відповідь
            string response = $"Привіт від сервера! Відпрвлено по адресі {clientEP.Address}:{clientEP.Port}!";

            // Кодуємо відповідь у байти
            byte[] responseData = Encoding.UTF8.GetBytes(response);

            // Відправляємо клієнту назад по тій самій адресі
            server.Send(responseData, responseData.Length, clientEP);
        }
    }
}
