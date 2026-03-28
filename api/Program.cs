using Microsoft.EntityFrameworkCore;
using Application;
using Application.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB CONNECTION (CHANGE SERVER IF NEEDED)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=localhost;Database=ClinicBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"));

// DI
builder.Services.AddScoped<IBooking, BookingService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();