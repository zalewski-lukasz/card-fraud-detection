using TransactionSimulator.BackgroundWorkers;
using TransactionSimulator.Repositories.Implementations;
using TransactionSimulator.Repositories.Interfaces;
using TransactionSimulator.Services.Implementations;
using TransactionSimulator.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ICardRepository, CardRepository>();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();

builder.Services.AddSingleton<IDataGeneratorService, DataGeneratorService>();
builder.Services.AddSingleton<IDataManagementService, DataManagementService>();

builder.Services.AddHostedService<DataProducerService>().AddSingleton<DataProducerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
