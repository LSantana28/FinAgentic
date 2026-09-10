using CobrAI.Data;
using CobrAI.Repositories;
using CobrAI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<OracleConnectionFactory>();
builder.Services.AddScoped<FaturaRepository>();
builder.Services.AddScoped<CobrancaService>();
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<IAService>();
builder.Services.AddScoped<CobrancaRepository>();
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();