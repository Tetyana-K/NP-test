using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        UdpClient client = new UdpClient();
        var serverEP = new IPEndPoint(IPAddress.Loopback, 5001); // localhost

        Console.WriteLine("UDP-клієнт запущено...\nВведіть назву запчастини (CPU, RAM, GPU, SSD ) або 'exit' для виходу.");

        while (true)
        {
            Console.Write("Запит: ");
            string input = Console.ReadLine()?.Trim() ?? "CPU";
            if (input.ToLower() == "exit") break;

            byte[] data = Encoding.UTF8.GetBytes(input);
            await client.SendAsync(data, data.Length, serverEP);

            var result = await client.ReceiveAsync();
            string response = Encoding.UTF8.GetString(result.Buffer);
            Console.WriteLine($"Відповідь сервера: {response}");
        }
    }
}
