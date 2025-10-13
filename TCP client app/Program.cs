using System;
using System.Net.Sockets;
using System.Text;

class TcpClientExample
{
    static void Main()
    {
        // Підключення до сервера (localhost, порт 5000)
        TcpClient client = new TcpClient("127.0.0.1", 5000);
        Console.WriteLine("Підключено до сервера.");

        // Відправляємо повідомлення
        NetworkStream stream = client.GetStream();
        Console.WriteLine("Введи повідомлення для сервера");
        //string text = "Привіт, сервер!";
        string text = Console.ReadLine()!;
        byte[] data = Encoding.UTF8.GetBytes(text);
        stream.Write(data, 0, data.Length);

        // Читаємо відповідь
        byte[] buffer = new byte[256];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        Console.WriteLine("Від сервера: " + Encoding.UTF8.GetString(buffer, 0, bytesRead));

        // Закриваємо з’єднання
        client.Close();
    }
}
