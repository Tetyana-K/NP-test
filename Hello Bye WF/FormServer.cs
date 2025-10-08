using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hello_Bye_WF
{
    public partial class FormServer : Form
    {
        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        private bool isRunning;
        int port = 5000;
        public FormServer()
        {
            InitializeComponent();
            rtbLog.ReadOnly = true;
            //btnStop.Enabled = false;
            //btnSend.Enabled = false;
            SetButtonsIdleState();
            isRunning = false;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            AppendLog($"Сервер слухає на порту {port}");
            isRunning = true;
            SetButtonsRunningState();

            client = await listener.AcceptTcpClientAsync();
            stream = client.GetStream();
            AppendLog("Клієнт підключився");

            _ = ReceiveMessageLoop(); // викликаємо метод із циклом читання повідомлень
        }

        private async Task ReceiveMessageLoop()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (isRunning) // цикл читання повідомлень
                {
                    // асинхронно читаємо дані з мережевого потоку
                    // метод чекає, поки не з’являться байти (або підключення не закриється)
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) // якщо прочитано 0 байт — клієнт закрив з’єднання
                        break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AppendLog($"Клієнт: {message}"); // пишемо повідомлення у текстовий бокс з логами

                    // якщо клієнт відправив "bye" — завершуємо підключення
                    if (string.Compare(message, "bye", true) == 0)
                    {
                        AppendLog("Клієнт попрощався. Закриваємо з'єднання...");
                        CloseConnection();
                        SetButtonsIdleState();
                        break;
                    }
                }
            }
            catch (IOException) // виникає, якщо потік несподівано закрився (наприклад, клієнт вийшов)
            {
                if(isRunning)
                    AppendLog("Підключення завершено (IOException).");
            }
            catch (ObjectDisposedException) // потік уже був закритий вручну (CloseConnection викликано раніше)
            {
                if(isRunning)
                    AppendLog("Потік уже закрито (ObjectDisposedException).");
            }
            catch (NullReferenceException)
            {
                AppendLog("Попередження: потік або клієнт закрито (NullReferenceException).");
            }
            catch (Exception ex)
            {
                AppendLog($"Помилка :{ex.Message}");
            }
        }

        private void CloseConnection()
        {
            stream?.Close();
            client?.Close();
            listener?.Stop();
            AppendLog("З’єднання завершено");
        }

        private void AppendLog(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (InvokeRequired)
                {

                    Invoke(new Action(() =>
                    {
                        rtbLog.AppendText(text + Environment.NewLine);
                    }));
                }
                else
                {
                    rtbLog.AppendText(text + Environment.NewLine);
                }
            }
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            isRunning = false; // важливо щробити це раніше, щоб вимкнути цикл у асинхронному ReceiveLoop()
           
            try
            {
                if (stream != null && client?.Connected == true)
                {
                    string msg = "Сервер вимкнувся";
                    byte[] data = Encoding.UTF8.GetBytes(msg);
                    await stream.WriteAsync(data, 0, data.Length);
                }
                else
                {
                    AppendLog("Немає активного клієнта — просто зупиняємо сервер.");
                }
            }
            catch { } // ігноруємо, якщо потік уже закритий

            CloseConnection();
            SetButtonsIdleState();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMessage.Text) || stream == null)
                return;

            string msg = tbMessage.Text.Trim();
            byte[] data = Encoding.UTF8.GetBytes(msg);
            await stream.WriteAsync(data, 0, data.Length);

            AppendLog("Сервер: " + msg);

            if (string.Compare(msg, "bye", true) == 0)
            {
                AppendLog("Ви попрощались із клієнтом. Закриваємо з’єднання...");
                CloseConnection();
                SetButtonsIdleState();
            }

            tbMessage.Clear();

        }
        private void SetButtonsIdleState() // зміна доступності кнопок, коли сервер зупинений
        {
            btnStart.Enabled = true;
            btnSend.Enabled = false;
            btnStop.Enabled = false;
        }
        private void SetButtonsRunningState() // зміна доступності кнопок, коли сервер запущений
        {
            btnStart.Enabled = false;
            btnSend.Enabled = true;
            btnStop.Enabled = true;
        }

    }
}
