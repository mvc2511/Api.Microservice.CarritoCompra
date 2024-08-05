using Api.Microservice.CarritoCompra.Persistencia;
using Api.Microservice.CarritoCompra.RemoteInterface;
using Api.Microservice.CarritoCompra.RemoteService;
using MediatR;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Registro de MediatR
builder.Services.AddMediatR(typeof(Program).Assembly); // Registra MediatR utilizando el assembly donde se encuentra Program

// Registro de ILibroService y su implementación
builder.Services.AddScoped<ILibroService, LibrosService>(); // Registra la implementación del servicio

// Registro de IHttpClientFactory
builder.Services.AddHttpClient("Libros", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Libros"]); // Configura la URL base de la API
});
// Configuración de HttpClient para el servicio de "Autores"
builder.Services.AddHttpClient("Autores", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Autores"]); // Configura la URL base de la API de Autores
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CarritoContexto>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Añadir configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
    builder => builder
    .WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod());
});

var app = builder.Build();

// Aplicar política CORS
app.UseCors("AllowLocalhost");

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