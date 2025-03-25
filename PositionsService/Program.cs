using Microsoft.EntityFrameworkCore;
using PositionsService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// SQLite database configuration
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()  // Allow any origin (only for the testing purpose, it should not be done on production environment)
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();

    context.Database.ExecuteSqlRaw("PRAGMA wal_checkpoint(TRUNCATE);");

    SeedData.Initialize(services, context);  // Llamamos al SeedData
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
