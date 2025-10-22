using HtmlAgilityPack;
using System;
using System.Linq;
using System.Xml;

class Program
{
    static void Main()
    {
        string url = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";

        var web = new HtmlWeb();
        var doc = web.Load(url);

        // Знаходимо таблицю з класом 'wikitable' (часто використовується для версій)
        var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class,'wikitable')]"); // знаходить таблицю на будь-якому рівні документа

        if (table == null)
        {
            Console.WriteLine("Таблицю не знайдено!");
            return;
        }

        // Беремо усі рядки
        var rows = table.SelectNodes(".//tr");

        foreach (var row in rows)
        {
            var cells = row.SelectNodes(".//th|.//td"); // вибираємо всі клітинки <th> або <td> в рядку
            if (cells != null)
            {
                // Робимо деентітізацію HTML символів, тобто перетворюємо &amp; в &, &lt; в < і т.д.
                var cellTexts = cells.Select(c => HtmlEntity.DeEntitize(c.InnerText.Trim()));
                Console.WriteLine(string.Join(" | ", cellTexts));
                Console.WriteLine(HtmlEntity.DeEntitize(cells[0].InnerText.Trim()));
                //Console.WriteLine((cells[0].InnerText.Trim()));
            }
        }
    }
}
