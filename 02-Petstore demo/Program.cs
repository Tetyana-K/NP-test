using System.Text;
using System.Text.Json;

HttpClient client = new HttpClient();
client.BaseAddress = new Uri("https://petstore.swagger.io/v2/"); // базова адреса API Petstore, тобто до цієї адреси будуть додаватися відносні шляхи в запитах

var pet = new { id = 123456, name = "Murchik", status = "available" }; // створюємо об'єкт тваринки анонімного типу для відправки в запиті
string json = JsonSerializer.Serialize(pet);
Console.WriteLine($"json for animal\n{json}\n");

var content = new StringContent(json, Encoding.UTF8, "application/json");

// POST - створення нового ресурсу (тваринки)
var postResponse = await client.PostAsync("pet", content); //  до базової адреси додається
                                                           //  відносний шлях "pet", тобто вийде
                                                           //  повна адреса https://petstore.swagger.io/v2/pet
Console.WriteLine("POST response:");
Console.WriteLine($"Status code : {postResponse.StatusCode}");
Console.WriteLine(await postResponse.Content.ReadAsStringAsync());

// GET - отримання інформації про ресурс (тваринку) за її ідентифікатором
var getResponse = await client.GetAsync("pet/123456"); //  до базової адреси додається
                                                       //  відносний шлях "pet/123456", тобто вийде
                                                       //  повна адреса https://petstore.swagger.io/v2/pet/123456
Console.WriteLine("GET response:");
Console.WriteLine($"Status code : {postResponse.StatusCode}");
Console.WriteLine(await getResponse.Content.ReadAsStringAsync());

var updatedPet = new { id = 123456, name = "Murchik", status = "sold", tags = new[] { "white" } };
string updatedJson = JsonSerializer.Serialize(updatedPet);
var updatedContent = new StringContent(updatedJson, Encoding.UTF8, "application/json");
// PUT - оновлення інформації про ресурс (тваринку)
var putResponse = await client.PutAsync("pet", updatedContent); //  до базової адреси додається
                                                                //  відносний шлях "pet", тобто вийде
                                                                //  повна адреса https://petstore.swagger.io/v2/pet
Console.WriteLine("PUT response:");
Console.WriteLine($"Status code : {postResponse.StatusCode}");
Console.WriteLine(await putResponse.Content.ReadAsStringAsync());


// https://petstore.swagger.io/v2/ - базова адреса API Petstore, це офіційний демонстраційний сервер Swagger для тестування API
// API = Application Programming Interface,
// інтерфейс програмування додатків, набір правил і протоколів для взаємодії між різними
// програмними компонентами або сервісами через мережу.