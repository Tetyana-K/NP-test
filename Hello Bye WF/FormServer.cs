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
            AppendLog($"������ ����� �� ����� {port}");
            isRunning = true;
            SetButtonsRunningState();

            client = await listener.AcceptTcpClientAsync();
            stream = client.GetStream();
            AppendLog("�볺�� ����������");

            _ = ReceiveMessageLoop(); // ��������� ����� �� ������ ������� ����������
        }

        private async Task ReceiveMessageLoop()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (isRunning) // ���� ������� ����������
                {
                    // ���������� ������ ��� � ���������� ������
                    // ����� ����, ���� �� ��������� ����� (��� ���������� �� ���������)
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) // ���� ��������� 0 ���� � �볺�� ������ 璺������
                        break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AppendLog($"�볺��: {message}"); // ������ ����������� � ��������� ���� � ������

                    // ���� �볺�� �������� "bye" � ��������� ����������
                    if (string.Compare(message, "bye", true) == 0)
                    {
                        AppendLog("�볺�� ����������. ��������� �'�������...");
                        CloseConnection();
                        SetButtonsIdleState();
                        break;
                    }
                }
            }
            catch (IOException) // ������, ���� ���� ���������� �������� (���������, �볺�� ������)
            {
                if(isRunning)
                    AppendLog("ϳ��������� ��������� (IOException).");
            }
            catch (ObjectDisposedException) // ���� ��� ��� �������� ������ (CloseConnection ��������� �����)
            {
                if(isRunning)
                    AppendLog("���� ��� ������� (ObjectDisposedException).");
            }
            catch (NullReferenceException)
            {
                AppendLog("������������: ���� ��� �볺�� ������� (NullReferenceException).");
            }
            catch (Exception ex)
            {
                AppendLog($"������� :{ex.Message}");
            }
        }

        private void CloseConnection()
        {
            stream?.Close();
            client?.Close();
            listener?.Stop();
            AppendLog("ǒ������� ���������");
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
            isRunning = false; // ������� ������� �� �����, ��� �������� ���� � ������������ ReceiveLoop()
           
            try
            {
                if (stream != null && client?.Connected == true)
                {
                    string msg = "������ ���������";
                    byte[] data = Encoding.UTF8.GetBytes(msg);
                    await stream.WriteAsync(data, 0, data.Length);
                }
                else
                {
                    AppendLog("���� ��������� �볺��� � ������ ��������� ������.");
                }
            }
            catch { } // ��������, ���� ���� ��� ��������

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

            AppendLog("������: " + msg);

            if (string.Compare(msg, "bye", true) == 0)
            {
                AppendLog("�� ����������� �� �볺����. ��������� 璺������...");
                CloseConnection();
                SetButtonsIdleState();
            }

            tbMessage.Clear();

        }
        private void SetButtonsIdleState() // ���� ���������� ������, ���� ������ ���������
        {
            btnStart.Enabled = true;
            btnSend.Enabled = false;
            btnStop.Enabled = false;
        }
        private void SetButtonsRunningState() // ���� ���������� ������, ���� ������ ���������
        {
            btnStart.Enabled = false;
            btnSend.Enabled = true;
            btnStop.Enabled = true;
        }

    }
}
