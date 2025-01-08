using MiningVehicle.API.Services;
using MiningVehicle.Extensions;
using MiningVehicle.Infrastructure.Repositories;
using MiningVehicle.SignalR.VehicleHub;
using MiningVehicle.VehicleEmulator;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.ConfigureCors();
builder.Services.ConfigureLoggerManager();

// API
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Vehicle Emulator
builder.Services.AddSingleton<IMiningVehicleClient, MiningVehicleClient>();
builder.Services.AddMiningVehicle(builder.Configuration);

// Hubs
builder.Services.AddAzureSignalRHub(builder.Configuration);
builder.Services.AddSignalRClients(builder.Configuration);

// Infrastructure
builder.Services.AddMongoDatabase(builder.Configuration);
builder.Services.AddSingleton<IRepository, VehicleDataRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<VehicleDataHub>("/vehicleDataHub");
app.MapControllers();

app.Run();


