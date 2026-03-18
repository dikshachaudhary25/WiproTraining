using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MyApiApp.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Get connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("AzureDB")
    ?? throw new InvalidOperationException("Connection string 'AzureDB' not found.");

// ✅ Register DbContext
builder.Services.AddDbContext<EmpContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ Add controller support
builder.Services.AddControllers();

// ✅ OpenAPI / Swagger
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

var app = builder.Build();

// ✅ Enable Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();

}

app.UseHttpsRedirection();

app.UseAuthorization();

// ✅ Map controllers
app.MapControllers();

app.Run();