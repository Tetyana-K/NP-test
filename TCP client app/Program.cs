using System;
using System.Net.Sockets;
using System.Text;

class TcpClientExample
{
    static void Main()
    {
        // 1. Підключення до сервера (localhost, порт 5000)
        TcpClient client = new TcpClient("127.0.0.1", 5000);
        Console.WriteLine("Підключено до сервера.");

        // 2. Відправляємо повідомлення
        NetworkStream stream = client.GetStream();
        Console.WriteLine("Введи повідомлення для сервера");
        //string text = "Привіт, сервер!";
        string text = Console.ReadLine()!;
        byte[] data = Encoding.UTF8.GetBytes(text);
        stream.Write(data, 0, data.Length);

        // 3. Читаємо відповідь
        byte[] buffer = new byte[256];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        Console.WriteLine("Від сервера: " + Encoding.UTF8.GetString(buffer, 0, bytesRead));

        // 4. Закриваємо з’єднання
        client.Close();
    }
}
