using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hello_Bye_WF_Client
{
    public partial class FormClient : Form
    {
        private TcpClient? client;
        private NetworkStream? stream;
        public FormClient()
        {
            InitializeComponent();
            rtbClientLogs.ReadOnly = true;
            tbIP.Text = "127.0.0.1";
            tbPort.Text = "5000";
        }
        private async void btnClientConnect_Click(object sender, EventArgs e)
        {
            var ip = tbIP.Text;
            var port = int.Parse(tbPort.Text);

            client = new TcpClient();// можна так client = new TcpClient(ip, port);
            try
            {
                await client.ConnectAsync(ip, port); // так більш безпечно
                stream = client.GetStream();
                AppendLog("Client is connected to server\r\n");

                await ReceiveClientMessageLoop();
            }
            catch (SocketException ex)
            {
                AppendLog("Server unavailable. Check IP address and port.");
                client = null;
            }
            catch (Exception ex)
            {
                AppendLog($"Connection error: {ex.Message}");
                client = null;
            }

        }

        private async Task ReceiveClientMessageLoop()
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
                    AppendLog($"Server: {message}");

                    if (IsMessageEqualsBye(message))
                    {
                        CloseConnection();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseConnection()
        {
            stream.Close();
            client.Close();
        }

        private void AppendLog(string log)
        {
            if (string.IsNullOrWhiteSpace(log))
                return;
            if (rtbClientLogs.InvokeRequired)
            {
                rtbClientLogs.Invoke(new Action(() => rtbClientLogs.AppendText(log + Environment.NewLine)));
            }
            else
            {
                rtbClientLogs.AppendText(log + Environment.NewLine);
            }
        }

        private async void btnClientSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbClientMessage.Text))
                return;
           
            if (stream == null) return;

            string msg = tbClientMessage.Text;
            byte[] data = Encoding.UTF8.GetBytes(msg);
            await stream.WriteAsync(data, 0, data.Length);

            AppendLog("Client: " + msg);

            if (IsMessageEqualsBye(msg))
            {
                CloseConnection();
            }

            tbClientMessage.Clear();
        }
        private bool IsMessageEqualsBye(string message) => string.Compare(message, "bye", true) == 0;


    }
}
