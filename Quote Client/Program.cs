using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class AsyncQuoteClient
{
    static async Task Main()
    {
        try
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 5000);
            Console.WriteLine("Підключено до сервера. Введіть 'exit' для виходу.");

            NetworkStream stream = client.GetStream();

            while (true)
            {
                Console.Write("Запитати цитату? (натисніть Enter) ");
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    byte[] exitMsg = Encoding.UTF8.GetBytes("exit");
                    await stream.WriteAsync(exitMsg, 0, exitMsg.Length);
                    break;
                }

                // Надсилаємо порожній запит
                byte[] request = Encoding.UTF8.GetBytes("\n");
                await stream.WriteAsync(request, 0, request.Length);

                // Читаємо відповідь
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("[Сервер]: " + response.Trim());
            }

            Console.WriteLine("Відключено від сервера.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }
    }
}
