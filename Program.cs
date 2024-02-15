using HttpClientExample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddHttpClient();

//builder.Services.AddHttpClient("weather", client =>
//{
//    client.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");   
//});

builder.Services.AddHttpClient<IWeatherService,WeatherService>(client =>
{
    client.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");   
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
