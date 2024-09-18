using Microsoft.EntityFrameworkCore;
using NWBackendAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Dependency Injektiolla välitetty tietokantatieto kontrollereille
builder.Services.AddDbContext<northwindOriginalContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("paikallinen")
    // builder.Configuration.GetConnectionString("pilvi") //KÄYTÄ TÄTÄ JOS AZUREEN JULKAISET
    ));
// ------------- Cors määritys ------------
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

app.UseCors("all"); //CORS KÄYTTÖÖNOTTO

app.UseAuthorization();

app.MapControllers();

app.Run();
