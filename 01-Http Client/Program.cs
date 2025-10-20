using System.Net;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

//await UseHttpClient();
//await UseHttpWebRequest();
//await PostRequestDemo();
//await PostRequestWithJsonDemo();
await TimeOutDemo();
async Task UseHttpClient()
{
    // Використання HttpClient для отримання HTML-коду сторінки за вказаною URL-адресою
    HttpClient client = new HttpClient(); // створили екземпляр HttpClient для роботи з HTTP-запитами

    //string url = "https://jsonplaceholder.typicode.com/posts/1";
    //string url = "http://w3schools.com";
    string url = "https://www.w3schools.com/xml/simple.xml"; // URL для отримання HTML-коду сторінки
    //string url = "https://httpbin.org/#/Status_codes"; // URL для отримання HTML-коду сторінки

    string html =  await client.GetStringAsync(url); // відправляємо GET-запит і отримуємо відповідь у вигляді рядка

    Console.WriteLine("Received HTML:");
    Console.WriteLine(html);
    Console.WriteLine("_____________________");
}

async Task UseHttpWebRequest()
{
    // Використання HttpWebRequest для отримання HTML-коду сторінки за вказаною URL-адресою, застарілий спосіб, розгдянемо для розуміння, що під капотом
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://jsonplaceholder.typicode.com/posts/1"); // створюємо запит
    HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());// отримуємо відповідь, obsolete
    Console.WriteLine($"Status code: {response.StatusCode}");
    Console.WriteLine($"Content length: {response.ContentLength}");
    Console.WriteLine($"Content type: {response.ContentType}");
    Console.WriteLine($"Content encoding: {response.ContentEncoding}");
    Console.WriteLine($"Content headers: {response.Headers}");
    Console.WriteLine($"Protocol: {response.ProtocolVersion}");

    using (StreamReader reader = new StreamReader(response.GetResponseStream())) // відкриваємо потік відповіді
    {
        string responseText = await reader.ReadToEndAsync(); // читаємо потік відповіді асинхронно
        Console.WriteLine("Response using HttpWebRequest:");
        Console.WriteLine(responseText);
    }
    Console.WriteLine("_____________________");
}

async Task PostRequestDemo()
{
    HttpClient client = new HttpClient();
    var data = new FormUrlEncodedContent(new[] // створюємо дані для POST-запиту, FormUrlEncodedContent кодує дані у форматі application/x-www-form-urlencoded
    {
        new KeyValuePair<string, string>("student", "Petro"),
        new KeyValuePair<string, string>("score", "97")
    });

    string url = "https://httpbin.org/post";
    var response = await client.PostAsync(url, data);
    
    //if(response.StatusCode == HttpStatusCode.OK)
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Post successful!");
        Console.WriteLine(await response.Content.ReadAsStringAsync()); // виводимо відповідь сервера за результатом POST-запиту
    }
    else
    {
        Console.WriteLine("Post failed.");
    }
}

async Task PostRequestWithJsonDemo()
{
    HttpClient client = new HttpClient();
    var jsonData = new StringContent(
        @"{""student"":""Ivan"",""score"":85}", // JSON-рядок {"student":"Ivan","score":85}
        System.Text.Encoding.UTF8,
        "application/json"); // створюємо JSON-дані для POST-запиту
    string url = "https://httpbin.org/post";
    var response = await client.PostAsync(url, jsonData); // відправляємо POST-запит синхронно
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Post successful!");
        Console.WriteLine(await response.Content.ReadAsStringAsync()); // виводимо відповідь сервера за результатом POST-запиту
    }
    else
    {
        Console.WriteLine("Post failed.");
    }
}

async Task TimeOutDemo()
{
    HttpClient client = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(2) // встановлюємо таймаут у 2 секунди
    };
    try
    {
        var response = await client.GetAsync("https://httpbin.org/delay/7"); // цей запит затримає відповідь на 5 секунд
        Console.WriteLine("Request succeeded.");
    }
    catch (TaskCanceledException) // виняток про таймаут викидається всередині завдання (Task), коли воно завершується невдало, тому ми ловимо TaskCanceledException
    {
        Console.WriteLine("Request timed out.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Request failed: {ex.Message}");
    }
}
// У прикладах використовується сайт httpbin.org - безкоштовний сервіс для тестування HTTP-запитів.
// httpbin.org  це 'дзеркало', все, що  відправляємо POST, GET, PUT, DELETE — він повертає назад, щоб можна було перевірити.
//Це не база даних, тому там нічого реально не зберігається, сайт для демо роботи з HTTP-запитами.

//https://jsonplaceholder.typicode.com - публічний фейковий онлайн  API для тестування.
//GET: /todos/1 - отримати завдання
//POST: /posts  -  створити новий запис