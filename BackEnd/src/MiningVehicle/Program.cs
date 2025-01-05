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
builder.Services.AddSignalR().AddAzureSignalR(System.Environment.GetEnvironmentVariable("SignalR:ConnectionString"));

// builder.Services.AddSignalR(e => {
//     e.EnableDetailedErrors = true;
//     e.MaximumReceiveMessageSize = 102400000;
// });
builder.Services.AddSignalRClients(builder.Configuration);

// Infrastructure
builder.Services.AddMongoDatabase(builder.Configuration);
builder.Services.AddScoped<IRepository, VehicleDataRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Development environment detected");
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine("Mapping SignalR Hub...");
app.MapHub<VehicleDataHub>("/vehicleDataHub");
app.MapControllers();

Console.WriteLine("Application pipeline configured. Running...");
app.Run();


