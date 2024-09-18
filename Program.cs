using Microsoft.EntityFrameworkCore;
using NWBackendAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Dependency Injektiolla v�litetty tietokantatieto kontrollereille
builder.Services.AddDbContext<northwindOriginalContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("paikallinen")
    // builder.Configuration.GetConnectionString("pilvi") //K�YT� T�T� JOS AZUREEN JULKAISET
    ));
// ------------- Cors m��ritys ------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("all",
    builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("all"); //CORS K�YTT��NOTTO

app.UseAuthorization();

app.MapControllers();

app.Run();
