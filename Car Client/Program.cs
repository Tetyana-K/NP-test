//Клієнтська частина для взаємодії з API автомобілів.
/*
 Клієнт надсилає 
    POST /cars
    Content-Type: application/json
    {"brand": "BMW", "model": "X5", "year": 2020}

 Сервер відповідає:
    HTTP/1.1 200 OK
    Content-Type: application/json
    {"status": "saved", "id": 3}
*/
using CarLibrary;
using System.Text;
using System.Text.Json;

class CarClient
{
    static async Task Main()
    {
        HttpClient client = new HttpClient(); // створюємо екземпляр HttpClient для відправки HTTP-запитів

        var cars = new[] // масив авто для додавання на сервер
        {
            new Car { Id = 1, Brand = "Toyota", Model = "Corolla Cross", Year = 2024 },
            new Car { Id = 2, Brand = "BMW", Model = "X5", Year = 2020 },
            new Car { Id = 3, Brand = "Audi", Model = "Q5", Year = 2025 }
        };

        foreach (var car in cars) // цикл для додавання кожного авто на сервер
        {
            string json = JsonSerializer.Serialize(car); // серіалізація об’єкта Car у формат JSON
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // створюємо вміст запиту з JSON-даними
            var response = await client.PostAsync("http://localhost:8080/cars", content); // відправляємо POST-запит на сервер для додавання авто
            Console.WriteLine(await response.Content.ReadAsStringAsync()); // виводимо відповідь сервера
        }

        Console.WriteLine("\nОтримуємо список авто (GET):");
        var listResponse = await client.GetAsync("http://localhost:8080/cars");
        string jsonResult = await listResponse.Content.ReadAsStringAsync();

        // десеріалізація назад у список об’єктів Car
        _ = ParseAndPrintCars(jsonResult);

        // оновлюємо машину з Id=2 (PUT)
        var updatedCar = new Car { Id = 2, Brand = "BMW", Model = "X6", Year = 2021 };
        string updatedJson = JsonSerializer.Serialize(updatedCar);
        var putContent = new StringContent(updatedJson, Encoding.UTF8, "application/json");

        Console.WriteLine("\nОновлюємо авто з Id = 2...(PUT)");
        var putResponse = await client.PutAsync("http://localhost:8080/cars/2", putContent);
        Console.WriteLine("\nРезультат PUT:");
        Console.WriteLine(await putResponse.Content.ReadAsStringAsync());

        Console.WriteLine("\nОтримуємо список авто (GET):");
        listResponse = await client.GetAsync("http://localhost:8080/cars");
        jsonResult = await listResponse.Content.ReadAsStringAsync();
        _ = ParseAndPrintCars(jsonResult);

    }

    private static Car[]? ParseAndPrintCars(string jsonResult)
    {
        var allCars = JsonSerializer.Deserialize<Car[]>(jsonResult);
        if (allCars != null)
        {
            foreach (var c in allCars)
                Console.WriteLine($"{c.Id}: {c.Brand} {c.Model} ({c.Year})");
        }

        return allCars;
    }
}