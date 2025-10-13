using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class ClientInfo
{
   // public IPEndPoint EndPoint { get; set; } // сокет клієнта
    public DateTime LastActive { get; set; } // час останьої активносі клієнта
    public int Count { get; set; }
}

class Program
{
    static Dictionary<IPEndPoint, ClientInfo> clients = new(); // словник для збереження клієнтів
    static Dictionary<string, decimal> prices = new()
    {
        {"CPU", 299.99m},
        {"GPU", 499.50m},
        {"RAM", 89.99m},
        {"SSD", 129.99m},
        {"Motherboard", 199.99m}
    };

    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        UdpClient server = new UdpClient(5001);
        Console.WriteLine("Сервер запущено на порту 5001...");

        while (true)
        {
            UdpReceiveResult result = await server.ReceiveAsync();
            IPEndPoint clientEP = result.RemoteEndPoint;
            string message = Encoding.UTF8.GetString(result.Buffer).Trim();

            // додаємо нового клієнта до  clients, якщо ще немає
            if (!clients.ContainsKey(clientEP)) // якщо клієнт новий
            {
                // додаємо його до словника clients
                clients[clientEP] = new ClientInfo { /*EndPoint = clientEP,*/ LastActive = DateTime.Now };
                clients[clientEP].Count = 0;
                Console.WriteLine($"Новий клієнт: {clientEP} підключився, час {DateTime.Now}");
            }
            else
            {
                clients[clientEP].LastActive = DateTime.Now;
            }

            Console.WriteLine($"Отримано запит від {clientEP}: {message}");
            clients[clientEP].Count++;
            string response;
            if (clients[clientEP].Count > 2)
                response = $"You have limit 2 requests";
            else
            {
                // обробка запиту, дивимося чи є запчастина у колекції
                //string response;
                if (prices.TryGetValue(message, out decimal price))
                {
                    response = $"{message} коштує ${price}";
                }
                else
                {
                    response = $"Не знайдено запчастину: {message}";
                }
            }
                byte[] data = Encoding.UTF8.GetBytes(response);
                await server.SendAsync(data, data.Length, clientEP);
            
        }
    }
}
