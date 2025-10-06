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
            SetButtonsStartValues();
            isRunning = false;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            AppendLog($"Сервер слухає на порту {port}");
            isRunning = true;
            SetButtonsEndValues();

            client = await listener.AcceptTcpClientAsync();
            stream = client.GetStream();
            AppendLog("Клієнт підключився");

            


            _ = ReceiveMessageLoop(); // викликаємо метод із циклом читання повідомленб
        }

        private async Task ReceiveMessageLoop()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AppendLog($"Клієнт: {message}");
                    if (string.Compare(message, "bye", true) == 0)
                    {
                        AppendLog("Закриваємо з'єднання");
                        CloseConnection();
                        break;
                    }
                }
            }
            catch (IOException)
            {
                if(isRunning)
                    AppendLog("Підключення завершено (IOException).");
            }
            catch (ObjectDisposedException)
            {
                if(isRunning)
                    AppendLog("Потік уже закрито (ObjectDisposedException).");
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
            string msg = "Сервер вимкнувся";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            await stream.WriteAsync(data, 0, data.Length);
            
            CloseConnection();
            SetButtonsEndValues();

        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMessage.Text) || stream == null)
                return;

            string msg = tbMessage.Text.Trim();
            byte[] data = Encoding.UTF8.GetBytes(msg);
            await stream.WriteAsync(data, 0, data.Length);

            AppendLog("Сервер: " + msg);

            //if (string.Compare(msg, "bye", true) == 0)
            //{
            //    AppendLog("Ви попрощались із клієнтом. Закриваємо з’єднання...");
            //    CloseConnection();
            //}

            tbMessage.Clear();

        }
        private void SetButtonsStartValues()
        {
            btnStart.Enabled = true;
            btnSend.Enabled = false;
            btnStop.Enabled = false;
        }
        private void SetButtonsEndValues()
        {
            btnStart.Enabled = false;
            btnSend.Enabled = true;
            btnStop.Enabled = true;
        }

    }
}
