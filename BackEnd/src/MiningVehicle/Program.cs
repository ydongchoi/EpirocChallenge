using MiningVehicle.API.Services;
using MiningVehicle.Extensions;
using MiningVehicle.Infrastructure.Repositories;
using MiningVehicle.SignalR.VehicleHub;
using MiningVehicle.VehicleEmulator;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.ConfigureCors();

// API
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Vehicle Emulator
builder.Services.AddSingleton<IMiningVehicleClient, MiningVehicleClient>();
builder.Services.AddMiningVehicle(builder.Configuration);

// Hubs
builder.Services.AddSignalR();
builder.Services.AddSignalRClients();

// Infrastructure
builder.Services.AddMongoDatabase(builder.Configuration);
builder.Services.AddScoped<IRepository, VehicleDataRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowReactApp");

app.MapHub<VehicleDataHub>("/vehicleDataHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();


