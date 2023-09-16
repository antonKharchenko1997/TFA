using Microsoft.EntityFrameworkCore;
using TFA.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ForumDbContext>(opt =>
        opt.UseNpgsql(
            "User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=tfa;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;"),
    ServiceLifetime.Singleton);

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