using EmergencyRoom.Core.Repositories;
using EmergencyRoom.Core.Service;
using EmergencyRoom.Data;
using EmergencyRoom.Data.Repositories;
using EmergencyRoom.Service.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/////////////////////////////олап


builder.Services.AddScoped<IClientAttributeService, ClientAttributeService>();
builder.Services.AddScoped<IClientAttributeRepository, ClientAttributeRepository>();

builder.Services.AddSingleton<DataContext>();

///////////////////////
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
