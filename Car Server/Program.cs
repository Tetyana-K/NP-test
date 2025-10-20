using CarLibrary;
using System.Net;
using System.Text;
using System.Text.Json;

class CarServer
{
    static List<Car> cars = new List<Car>();

    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        HttpListener listener = new HttpListener(); // створюємо екземпляр HttpListener (сервер) для прослуховування HTTP-запитів
        listener.Prefixes.Add("http://localhost:8080/"); // додаємо префікс, за яким сервер буде приймати запити
        listener.Start(); // запускаємо сервер
        Console.WriteLine("Сервер запущено на http://localhost:8080/");
        Console.WriteLine("Доступні маршрути: POST /cars, GET /cars, PUT /cars");

        while (true)
        {
            var context = await listener.GetContextAsync(); // очікуємо вхідні запити асинхронно HttpListenerContext
            var request = context.Request; // отримуємо об'єкт запиту HttpListenerRequest
            var response = context.Response; // отримуємо об'єкт відповіді HttpListenerResponse
            LogRequest(request); // логування інформації про запит

            // Обробка маршруту для додавання нового авто (POST /cars)
            if (request.HttpMethod == "POST" && request?.Url?.AbsolutePath == "/cars")
            {
                // відкриваємо потік для читання тіла HTTP-запиту (наприклад, JSON, який надіслав клієнт)
                using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                string body = await reader.ReadToEndAsync(); // читаємо тіло запиту
                try
                {
                    Car? car = JsonSerializer.Deserialize<Car>(body);  // десеріалізуємо JSON у об'єкт Car
                    if (car != null) // перевіряємо, чи десеріалізація пройшла успішно
                    {
                        cars.Add(car); // додаємо авто до списку cars
                        Console.WriteLine($"Отримано авто: {car.Brand} {car.Model} ({car.Year})");

                        // формуємо тіло відповіді у форматі JSON
                        var msg = JsonSerializer.Serialize(new { status = "saved", count = cars.Count });
                        byte[] buffer = Encoding.UTF8.GetBytes(msg);
                        response.ContentType = "application/json"; // кажемо, що відповідь у форматі JSON
                        await response.OutputStream.WriteAsync(buffer); // надсилаємо відповідь клієнту
                    }
                }
                catch (Exception ex)
                {
                    // Якщо сталася помилка під час десеріалізації, надсилаємо помилку клієнту
                    response.StatusCode = 400; // статус відповіді буде 400 Bad Request
                    byte[] buffer = Encoding.UTF8.GetBytes($"Invalid JSON: {ex.Message}");
                    await response.OutputStream.WriteAsync(buffer); // надсилаємо відповідь клієнту
                }
                response.Close(); // закриваємо відповідь
            }
            // Обробка сервером маршруту для отримання списку авто (GET /cars)
            else if (request?.HttpMethod == "GET" && request?.Url?.AbsolutePath == "/cars")
            {
                // серіалізуємо список авто cars у формат JSON
                string json = JsonSerializer.Serialize(cars,
                    new JsonSerializerOptions { WriteIndented = true }); //WriteIndented = true для гарного форматування JSON
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                response.ContentType = "application/json";
                await response.OutputStream.WriteAsync(buffer); // надсилаємо відповідь клієнту
                response.Close();
            }
            else if (request.HttpMethod == "PUT" && request.Url.AbsolutePath.StartsWith("/cars/"))
            {
                // отримуємо id з шляху
                string idStr = request.Url.AbsolutePath.Substring("/cars/".Length);
                if (int.TryParse(idStr, out int carId))
                {
                    var car = cars.Find(c => c.Id == carId);
                    if (car != null)
                    {
                        // читаємо тіло PUT-запиту
                        using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                        string body = await reader.ReadToEndAsync();
                        try
                        {
                            var updatedCar = JsonSerializer.Deserialize<Car>(body);
                            if (updatedCar != null)
                            {
                                // оновлюємо поля автомобіля
                                car.Brand = updatedCar.Brand;
                                car.Model = updatedCar.Model;
                                car.Year = updatedCar.Year;

                                string json = JsonSerializer.Serialize(new { status = "updated", car });
                                byte[] buffer = Encoding.UTF8.GetBytes(json);
                                response.ContentType = "application/json";
                                await response.OutputStream.WriteAsync(buffer);
                            }
                        }
                        catch (Exception ex)
                        {
                            response.StatusCode = 400;
                            byte[] buffer = Encoding.UTF8.GetBytes($"Invalid JSON: {ex.Message}");
                            await response.OutputStream.WriteAsync(buffer);
                        }
                    }
                    else
                    {
                        response.StatusCode = 404;
                        byte[] buffer = Encoding.UTF8.GetBytes($"Car with Id={carId} not found");
                        await response.OutputStream.WriteAsync(buffer);
                    }
                }
                else
                {
                    response.StatusCode = 400;
                    byte[] buffer = Encoding.UTF8.GetBytes("Invalid Id");
                    await response.OutputStream.WriteAsync(buffer);
                }

                response.Close();
            }

            else // якщо маршрут не знайдено, надсилаємо 404 Not Found
            {
                response.StatusCode = 404;
                byte[] buffer = Encoding.UTF8.GetBytes("Not found");
                await response.OutputStream.WriteAsync(buffer);
                response.Close(); // закриваємо відповідь Метод response.Close():

                //response.Close()
                //Закриває вихідний потік(OutputStream)
                //Виставляє коректні заголовки(наприклад, Content - Length)
                //Відправляє дані клієнту
                //Звільняє ресурси об’єкта HttpListenerResponse
            }
        }
    }
    static void LogRequest(HttpListenerRequest request)
    {
        Console.WriteLine($"Received {request.HttpMethod} request for {request.Url}");
        var url = request.Url;
        Console.WriteLine(url.AbsoluteUri);   // http://localhost:8080/cars?brand=BMW
        Console.WriteLine(url.Host);          // localhost
        Console.WriteLine(url.Port);          // 8080
        Console.WriteLine(url.AbsolutePath);  // /cars
        Console.WriteLine(url.Query);         // ?brand=BMW
        Console.WriteLine(url.Fragment);
        Console.WriteLine();
    }
}
//request.Url — це повний URL, який клієнт надіслав серверу, наприклад, http://localhost:8080/cars?id=1
//request.Url.AbsolutePath — це лише шлях частини URL, без домену, порту, параметрів запиту та фрагментів. У цьому випадку це буде /cars.