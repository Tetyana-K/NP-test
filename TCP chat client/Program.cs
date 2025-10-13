using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TcpChatClient
{
    static void Main()
    {
        TcpClient client = new TcpClient("127.0.0.1", 5000);
        NetworkStream stream = client.GetStream();

        Console.Write("Введіть своє ім’я: ");
        string name = Console.ReadLine();

        // потік для прийому повідомлень
        Thread receiveThread = new Thread(() =>
        {                                                         
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("\n" + msg);
                        Console.Write("> ");
                    }
                }
                catch { break; }
            }
        });
        receiveThread.Start();

        // відправлення повідомлень
        while (true)
        {
            Console.Write("> ");
            string message = Console.ReadLine();
            if (message.ToLower() == "exit") break;

            string fullMsg = $"{name}: {message}";
            byte[] data = Encoding.UTF8.GetBytes(fullMsg);
            stream.Write(data, 0, data.Length);
        }

        client.Close();
    }
}
