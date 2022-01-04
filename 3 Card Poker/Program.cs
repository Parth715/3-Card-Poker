using _3_Card_Poker;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var conn = "PokerDbContext";
builder.Services.AddDbContext<PokerDbContext>(x => { x.UseSqlServer(builder.Configuration.GetConnectionString(conn)); });
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
