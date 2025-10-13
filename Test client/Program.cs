using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class SimpleClient
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 5000);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(endPoint);
            Console.WriteLine("Підключено до сервера!");

            byte[] buffer = new byte[1024];

            // --- КРОК 1: надсилаємо "Привіт сервер"
            string hello = "Привіт сервер";
            client.Send(Encoding.UTF8.GetBytes(hello));
            Console.WriteLine("Клієнт: " + hello);

            // Отримуємо відповідь сервера
            int bytes = client.Receive(buffer);
            string reply1 = Encoding.UTF8.GetString(buffer, 0, bytes);
            Console.WriteLine("Сервер: " + reply1);

            // --- КРОК 2: вводимо ім’я
            Console.Write("Введи своє ім’я: ");
            string name = Console.ReadLine();
            client.Send(Encoding.UTF8.GetBytes(name));

            // Отримуємо фінальну відповідь
            bytes = client.Receive(buffer);
            string reply2 = Encoding.UTF8.GetString(buffer, 0, bytes);
            Console.WriteLine("Сервер: " + reply2);

            // Закриваємо з’єднання
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Помилка (SocketException): " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }
    }
}
