using AppCore.Module;
using AppCore.Repository;
using AppCore.Services;
using Infrastructure.Memory;
using WebApp.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidators();
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IVehicleRepository, MemoryVehicleRepository>();
builder.Services.AddSingleton<IParkingSessionRepositoryAsync, MemoryParkingSessionRepository>();
builder.Services.AddSingleton<IParkingGateRepository, MemoryParkingGateRepository>();
builder.Services.AddSingleton<ICameraCaptureRepository, MemoryCameraCaptureRepository>();
builder.Services.AddSingleton<IParkingUnitOfWork, MemoryParkingUnitOfWork>();
builder.Services.AddSingleton<IParkingGateService, MemoryParkingGateService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();