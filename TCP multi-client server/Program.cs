using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TcpChatServer
{
    static List<TcpClient> clients = new List<TcpClient>();
    static object locker = new object();

    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Сервер запущено. Очікування клієнтів...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            lock (locker) clients.Add(client);
            Console.WriteLine("Новий клієнт підключився.");

            // запускаємо клієнта в окремому потоці
            Thread thread = new Thread(HandleClient);
            thread.Start(client);
        }
    }

    static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // клієнт відключився

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Повідомлення: " + message);

                // розсилаємо всім іншим клієнтам
                Broadcast(message, client);
            }
        }
        catch { }
        finally
        {
            lock (locker) clients.Remove(client);
            client.Close();
            Console.WriteLine("Клієнт відключився.");
        }
    }

    static void Broadcast(string message, TcpClient sender)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);

        lock (locker)
        {
            foreach (var client in clients)
            {
                if (client != sender)
                {
                    try
                    {
                        client.GetStream().Write(data, 0, data.Length);
                    }
                    catch { }
                }
            }
        }
    }
}

