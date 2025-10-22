using HtmlAgilityPack; // dotnet add package HtmlAgilityPack
/*
 * Для складного веб-парсингу (де потрібно обробляти JavaScript) HtmlAgilityPack не підходить — він бачить лише сирий (static) HTML, без виконаного JS.
У такому разі використовують Selenium, PuppeteerSharp або Playwright.
 */

// the URL of the target Wikipedia page
string url = "https://en.wikipedia.org/wiki/C_Sharp_(programming_language)";

var web = new HtmlWeb();

// завантаження HTML-документа за вказаною URL-адресою
var document = web.Load(url);

// отримування всіх посилань на сторінці
//PrintHyperLinks(document);
//PrintImages(document);
ShowInfobox(document);

// знаходження таблиці інфобоксу (зверху справа на сайті) за класом "infobox vevent"
void ShowInfobox(HtmlDocument doc)
{

    var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class,'infobox')]");
    if (table == null)
    {
        Console.WriteLine("Infobox table not found.");
        return;
    }
    // Вибираємо всі рядки таблиці
    var rows = table.SelectNodes(".//tr"); // вибираємо всі елементи <tr> (рядки таблиці) в межах цієї таблиці
                                           // Xpath //tr вибирає всі рядки таблиць у документі
    if (rows == null)
    {
        Console.WriteLine("Table does not have rows.");
        return;
    }

    // Перебираємо всі рядки таблиці
    foreach (var row in rows)
    {
        var header = row.SelectSingleNode(".//th"); // шукаємо заголовок (th) у рядку
        var cell = row.SelectSingleNode(".//td"); // шукаємо значення (td) у рядку

        // Якщо є назва (th) і значення (td)
        if (header != null && cell != null)
        {
            Console.WriteLine($"{header.InnerText.Trim()}: {cell.InnerText.Trim()}"); //InnerText - отримує текст всередині елемента
        }
        // Якщо тільки заголовок (наприклад, назва секції)
        else if (header != null)
        {
            Console.WriteLine($"\n== {header.InnerText.Trim()} ==");
            Console.WriteLine($"\n>> {HtmlEntity.DeEntitize(header.InnerText.Trim())} >>");
        }
    }
}

static void PrintHyperLinks(HtmlDocument document)
{
    var links = document.DocumentNode.SelectNodes("//a[@href]"); // вибираємо всі елементи <a> з атрибутом href

    if (links != null)
    {
        foreach (var link in links)
            Console.WriteLine(link.GetAttributeValue("href", ""));
    }
}

static void PrintImages(HtmlDocument document)
{

    // отримування всіх зображень на сторінці
    var images = document.DocumentNode.SelectNodes("//img[@src]"); // вибираємо всі елементи <img> з атрибутом src
    if (images != null)
    {
        foreach (var img in images)
            Console.WriteLine(img.GetAttributeValue("src", ""));
    }
}
