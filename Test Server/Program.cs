using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class SimpleServer
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 5000);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        server.Bind(endPoint);
        server.Listen(1);
        Console.WriteLine("Сервер слухає на порту 5000...");

        Socket client = server.Accept();
        Console.WriteLine("Клієнт підключився!");

        byte[] buffer = new byte[1024];

        // --- КРОК 1: отримуємо "Привіт сервер"
        int bytes = client.Receive(buffer);
        string message1 = Encoding.UTF8.GetString(buffer, 0, bytes);
        Console.WriteLine("Клієнт: " + message1);

        // Відповідаємо: "Привіт клієнт! Як тебе звати?"
        string reply1 = "Привіт клієнт! Як тебе звати?";
        client.Send(Encoding.UTF8.GetBytes(reply1));
        Console.WriteLine("Сервер: " + reply1);

        // --- КРОК 2: отримуємо ім’я
        bytes = client.Receive(buffer);
        string name = Encoding.UTF8.GetString(buffer, 0, bytes);
        Console.WriteLine("Клієнт: " + name);

        // Відповідаємо: "Радий знайомству {ім’я}!"
        string reply2 = $"Радий знайомству {name}!";
        client.Send(Encoding.UTF8.GetBytes(reply2));
        Console.WriteLine("Сервер: " + reply2);

        // Закриваємо з’єднання
        client.Shutdown(SocketShutdown.Both);
        client.Close();
        server.Close();

        Console.WriteLine("Сервер завершив роботу.");
    }
}