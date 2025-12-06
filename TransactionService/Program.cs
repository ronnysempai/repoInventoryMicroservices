using Microsoft.EntityFrameworkCore;
using TransactionService.Data;


var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpClient para comunicar microservicios
builder.Services.AddHttpClient("ProductService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7103"); // cambiar si usas otro puerto
});

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
