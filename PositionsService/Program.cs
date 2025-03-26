using Microsoft.EntityFrameworkCore;
using PositionsService.Data;
using PositionsService.Hubs;
using PositionsService.Services;

var builder = WebApplication.CreateBuilder(args);

// SQLite database configuration
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();

builder.Services.AddSingleton<RabbitMqService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000")  // Allow specific origin
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

var rabbitMqService = app.Services.GetRequiredService<RabbitMqService>();
rabbitMqService.ConsumeMessages();  // Consuming messages from RabbitMQ


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();

    context.Database.ExecuteSqlRaw("PRAGMA wal_checkpoint(TRUNCATE);");

    context.Database.Migrate(); // Making sure the db and its migrations are applied

    SeedData.Initialize(services, context);  // Calling the SeedData
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PositionHub>("/positionHub");

app.Run();
