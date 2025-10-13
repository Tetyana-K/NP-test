using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpServer
{
    static void Main()
    {
        // Створюємо слухача (сервер) на порту 5000
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Сервер запущено. Очікування підключень...");

        while (true)
        {
            // Очікуємо клієнта
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Клієнт підключився!");

            // Отримуємо потік для обміну даними
            NetworkStream stream = client.GetStream();

            // Читаємо повідомлення від клієнта
            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Від клієнта: " + message);

            // Відправляємо відповідь
            byte[] response = Encoding.UTF8.GetBytes("Привіт від сервера!");
            stream.Write(response, 0, response.Length);

            // Закриваємо підключення
            client.Close();
        }
    }
}
