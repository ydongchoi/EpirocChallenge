using MiningVehicle.Extensions;
using MiningVehicle.VehicleEmulator;

var builder = WebApplication.CreateBuilder(args);

// Configuration

// Services
builder.Services.AddMiningVehicle(builder.Configuration);

// Test
var serviceProvider = builder.Services.BuildServiceProvider();
var miningVehicleEmulator = serviceProvider.GetService<MinigVehicleEmulator>();
miningVehicleEmulator.StartEngine();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
