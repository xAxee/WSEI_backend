using AppCore.Repository;
using AppCore.Services;
using Infrastructure.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IVehicleRepository, MemoryVehicleRepository>();
builder.Services.AddSingleton<IParkingSessionRepositoryAsync, MemoryParkingSessionRepository>();
builder.Services.AddSingleton<IParkingGateRepository, MemoryParkingGateRepository>();
builder.Services.AddSingleton<IParkingUnitOfWork, MemoryParkingUnitOfWork>();
builder.Services.AddSingleton<IParkingGateService, MemoryParkingGateService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();