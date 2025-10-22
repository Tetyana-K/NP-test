using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
/*
 dotnet add package HtmlAgilityPack - встановлення бібліотеки HtmlAgilityPack для парсингу HTML

 XPath - це мова запитів для вибору вузлів у XML-документі (HTML є підмножиною XML).
 Вона дозволяє навігувати по структурі документа і вибирати елементи на основі їхніх тегів, атрибутів, текстового вмісту та інших критеріїв.
 Основні поняття XPath:
 Вузли: Основні одиниці в XML/HTML-документі (елементи, атрибути, текст тощо).
 Вирази: Команди, які використовуються для вибору вузлів (наприклад, //a вибирає всі елементи <a>).
 Ось деякі приклади XPath:
 Вибір всіх елементів <a>: //a
 Вибір елементів<a> з атрибутом href: //a[@href]
 Вибір елементів<h2>: //h2
 Вибір елементів<img> з атрибутом src: //img[@src]
 Вибір елементів<table> з певним класом: //table[@class='classname']
 Вибір першого елемента <div>:   //div[1]
 Вибір останнього елемента <div>: //div[last()]
 Вибір елементів<p> всередині<div>: //div/p
 Вибір елементів<tr> всередині таблиці з id = "customers": //table[@id='customers']//tr
 Вибір елементів списку <li> всередині <ul> з класом "menu": //ul[@class='menu']/li
  
. означає поточний вузол
/ означає від кореня документа
  /html/body/table  шукає <table> безпосередньо під <body>, не в будь-якому іншому місці. Точний шлях, рівень за рівнем.

  // означає будь-де у документі
  //table  шукає усі таблиці в документі, на будь-якому рівні вкладеності.
  Наприклад, //table[contains(@class, 'infobox')]  шукає таблиці, клас яких містить слово infobox.

*/

class Program
{
    static async Task Main()
    {
        //string url = "https://news.ycombinator.com/";
       // string url = "https://www.gutenberg.org/";
        //string url = "https://learn.microsoft.com/en-us/dotnet/csharp/";
        //string url = "https://www.w3schools.com/tags/tryit.asp?filename=tryhtml5_video";
        //string url = "https://www.bbc.com";

        //HttpClient client = new HttpClient(); // створюємо екземпляр HttpClient для відправки HTTP-запитів
        //string html = await client.GetStringAsync(url); // відправляємо GET-запит і отримуємо HTML-код сторінки у вигляді рядка

        //HtmlDocument doc = new HtmlDocument(); // створюємо об'єкт HtmlDocument для парсингу HTML
        //doc.LoadHtml(html);// завантажуємо HTML-код у документ, формується DOM-дерево

        //// знаходження всіх посилань на сторінці
        //PrintInColor("Hyperlinks:", ConsoleColor.Green);
        //var nodes = doc.DocumentNode.SelectNodes("//a[@href]"); // вибираємо всі елементи <a> з атрибутом href
        //if (nodes != null)
        //{
        //    foreach (var node in nodes)
        //    {
        //        Console.WriteLine(node.InnerText);
        //    }
        //    PrintInColor($"{nodes.Count} hyperlinks found", ConsoleColor.Green);
        //}

        //// знаходження всіх заголовків h2 на сторінці
        //PrintInColor("Headings (h2):", ConsoleColor.Cyan);
        //if (doc.DocumentNode.SelectNodes("//h2") != null)
        //{
        //    foreach (var node in doc.DocumentNode.SelectNodes("//h2")) // вибираємо всі елементи <h2>
        //    {
        //        Console.WriteLine(node.InnerText);
        //    }
        //}

        //// знаходження всіх зображень на сторінці
        //PrintInColor("Images:", ConsoleColor.Yellow);
        //if (doc.DocumentNode.SelectNodes("//img[@src]") != null)
        //{
        //    foreach (var node in doc.DocumentNode.SelectNodes("//img[@src]")) // вибираємо всі елементи <img> з атрибутом src
        //    {
        //        Console.WriteLine(node.GetAttributeValue("src", "")); // виводимо значення атрибута src
        //    }
        //}

         await ParseTable();
    }
    static void PrintInColor(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    static async Task ParseTable()
    {
        string url = "https://www.w3schools.com/html/html_tables.asp";
        using HttpClient client = new HttpClient();
        string html = await client.GetStringAsync(url);

        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Вибираємо всі рядки таблиці
        var rows = doc.DocumentNode.SelectNodes("//table[@id='customers']//tr"); // таблиця з id="customers", вибираємо всі рядки <tr>, включаючи заголовок
        if (rows == null) 
            return;

        foreach (var row in rows) // ходимо по кожному рядку
        {
            var cells = row.SelectNodes("./td"); // вибираємо всі клітинки <td> в рядку
            if (cells != null)
            {
                Console.WriteLine($"{cells[0].InnerText:-30} | {cells[1].InnerText:-30} | {cells[2].InnerText:20}");
            }
        }
    }
}
