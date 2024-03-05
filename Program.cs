using Microsoft.EntityFrameworkCore;
using ProductApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
  options.ConfigureSwaggerForJwt();
});

// Register the database context
var connectionString = builder.Configuration.GetConnectionString("Product");
builder.Services.AddDatabase(connectionString);

// Register my services
builder.Services.Register();

// Add JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
// app.UseErrorHandlingMiddleware();
// app.UseSecurityMiddleware();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  Console.WriteLine("===IsDevelopment===");
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
