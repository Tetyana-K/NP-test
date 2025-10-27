using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; // Додай цей простір імен

namespace _06_Use_config
{
    public class SmtpSettings
    {
        // Використовуємо ConfigurationManager для отримання налаштувань з App.config
        public static string Server => ConfigurationManager.AppSettings["SmtpServer"]; // Отримуємо значення SmtpServer 
        public static int Port => int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
        public static bool EnableSsl => bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
    }
}
