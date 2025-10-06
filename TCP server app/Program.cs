using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpServer
{
    static void Main()
    {
        // 1. Створюємо слухач на порту 5000
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Сервер запущено. Очікування підключень...");

        while (true)
        {
            // 2. Очікуємо клієнта
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Клієнт підключився!");

            // 3. Отримуємо потік для обміну даними
            NetworkStream stream = client.GetStream();

            // 4. Читаємо повідомлення від клієнта
            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Від клієнта: " + message);

            // 5. Відправляємо відповідь
            byte[] response = Encoding.UTF8.GetBytes("Привіт від сервера!");
            stream.Write(response, 0, response.Length);

            // 6. Закриваємо підключення
            client.Close();
        }
    }
}
