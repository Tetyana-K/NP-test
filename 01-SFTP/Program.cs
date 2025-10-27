//доступний публічний тестовий SFTP-сервер  test.rebex.net, який можна використати для демонстрації:
//Хост: test.rebex.net, порт 22
//Логін / пароль: demo / password
//Обмеження: лише режим read - only(завантаження файлів), завантажувати чи видаляти файли не можна
// dotnet add package SSH.NET

using Renci.SshNet;

class Program
{
    static void Main()
    {
        string host = "localhost";
        //string host = "test.rebex.net";
        int port = 22; // Стандартний порт SFTP
        string username = "tania";
        string password = "123";
        //string username = "demo";
        //string password = "password";

        string remotePath = "/"; // Папка на сервері test.rebex.net
        //string remotePath = "/pub/example/"; // Папка на сервері test.rebex.net
        string localPath = "Downloads";      // Локальна папка для збереження файлів

        if (!Directory.Exists(localPath))
            Directory.CreateDirectory(localPath);

        using (var sftp = new SftpClient(host, port, username, password))
        {
            try
            {
                Console.WriteLine("Підключення до SFTP...");
                sftp.Connect();
                Console.WriteLine("Підключено до локального sfpt tiny сервера\n");
                //Console.WriteLine("Підключено до сервера test.rebex.net\n");

                // Перелічуємо файли на сервері
                var files = sftp.ListDirectory(remotePath);
                Console.WriteLine("Файли на сервері:");
                foreach (var file in files)
                {
                    if (!file.IsDirectory)
                        Console.WriteLine($"  - {file.Name}");
                }

                // Завантажимо один із файлів
                //string fileToDownload = "First/hello.txt";
                string fileToDownload = "demo file2.txt";
              //  string fileToDownload = "readme.txt";
                string remoteFile = remotePath + fileToDownload;
                string localFile = Path.Combine(localPath, fileToDownload);

                Console.WriteLine($"\nЗавантаження {fileToDownload} ...");
                using (var fileStream = File.Create(localFile))
                {
                    sftp.DownloadFile(remoteFile, fileStream);
                }

                Console.WriteLine($"Файл {fileToDownload} збережено у: {Path.GetFullPath(localFile)}");

                sftp.Disconnect();
                Console.WriteLine("\nЗ`єднання закрито");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
