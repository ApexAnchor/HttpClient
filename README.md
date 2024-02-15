# HttpClient
Sample project to demonstrate different ways of using http client
# Problems of using HttpClient directly without creating from IHttpClientFactory
# 1.Resource Exhaustion: 
Creating a new HttpClient for each request can lead to exhaustion of system resources, such as sockets, due to not efficiently reusing connections.
# 2.Socket Leakage:
Not properly managing the lifecycle of HttpClient instances may result in socket leakage. Failing to dispose of HttpClient instances can leave connections open, causing resource leaks.
# 3.Performance Overhead:
Frequent creation and disposal of HttpClient instances can introduce performance overhead, as it involves establishing new connections and tearing them down for each request.
# 4.DNS Resolution Issues:
DNS resolution may not be cached when using a new HttpClient for each request, potentially causing delays due to repeated DNS lookups.
# 5.Connection Pooling Problems:
Connection pooling, which helps efficiently reuse existing connections, is not utilized effectively when creating a new HttpClient for each request. This can impact the application's scalability.
# 6.Configuration Duplication:
Configuration settings, such as timeouts or default headers, need to be duplicated for each HttpClient instance, leading to code redundancy and increased chances of misconfigurations.
# 7.No Centralized Configuration:
Without using IHttpClientFactory, there is no centralized way to configure and manage HttpClient instances, making it harder to apply global changes consistently.
# 8.Limited Logging and Instrumentation:
It becomes challenging to implement consistent logging and instrumentation across all HTTP requests when not utilizing the factory. Centralized logging and instrumentation are often essential for debugging and monitoring.
# 9.Difficulty in Unit Testing:
Unit testing can be more challenging when HttpClient instances are directly instantiated, as it becomes harder to mock or replace them during testing.
# 10. Scoped Service Issues:
When using HttpClient directly in a scoped service, it might lead to unintended behavior, as HttpClient is designed to be long-lived. Utilizing IHttpClientFactory helps manage the lifecycle of HttpClient instances within a scoped service appropriately.

# HttpClient creation using IHttpClientFactory Pattern Examples

## 1. Simple HttpClient

The simple HttpClient is the most basic form, where you create an instance of `HttpClient` and use it to make HTTP requests.
```
 public async Task<IActionResult> GetFromSimpleClient([FromQuery] string cityName)
 {
    var client = clientFactory.CreateClient();
    string apiKey = config.GetValue<string>("ApiKey");
    string url = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={cityName}&aqi=yes";
    var response = await client.GetAsync(url);
    return new JsonResult(await response.Content.ReadAsStringAsync());          
 }
```

## 2. Named HttpClient

In the named HttpClient example, we set a base address for the client, making it easier to create relative URIs for requests. This can be beneficial when working with a specific API.
```
public async Task<IActionResult> GetFromNamedClient([FromQuery] string cityName)
{
    var client = clientFactory.CreateClient("weather");
    string apiKey = config.GetValue<string>("ApiKey");
    string url = $"?key={apiKey}&q={cityName}&aqi=yes";
    var response = await client.GetAsync(url);
    return new JsonResult(await response.Content.ReadAsStringAsync());
}
```
```
builder.Services.AddHttpClient("weather", client =>
{
    client.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");
});
```

## 3. Typed HttpClient

The typed HttpClient example demonstrates a more structured approach. We define a class `WeatherService` that encapsulates the logic for interacting with a specific API (`https://https://www.weatherapi.com)` in this case). This provides a type-safe way to make requests, as opposed to using generic `HttpClient` methods directly.

In this example, the `WeatherService` class has a method `GetWeatherData` that fetches weather data for a given city.

```
builder.Services.AddHttpClient<IWeatherService,WeatherService>(client =>
{
    client.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");   
});
```
```
public WeatherService(HttpClient httpClient,IConfiguration config)
{
    this.httpClient = httpClient;
    this.config = config;
}

public async Task<string> GetWeatherData(string cityName)
{
    string apiKey = config.GetValue<string>("ApiKey");
    string url = $"?key={apiKey}&q={cityName}&aqi=yes";
    var response = await httpClient.GetAsync(url);
    return await response.Content.ReadAsStringAsync();
}
```
These examples showcase different levels of abstraction and organization when using `HttpClient` in C#.
