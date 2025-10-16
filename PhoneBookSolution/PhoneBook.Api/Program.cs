using Microsoft.OpenApi.Models;
using PhoneBook.Application.Services;
using PhoneBook.Domain.Interfaces;
using PhoneBook.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhoneBook API", Version = "v1" });
});

builder.Services.AddSingleton<IPhoneBookRepository, InMemoryPhoneBookRepository>();
builder.Services.AddScoped<IPhoneBookService, PhoneBookService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhoneBook API v1"));

app.UseHttpsRedirection();
app.MapControllers();

app.Run();