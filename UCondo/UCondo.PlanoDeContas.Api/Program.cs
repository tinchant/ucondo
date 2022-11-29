using Microsoft.EntityFrameworkCore;
using UCondo.PlanoDeContas.Domain.AgregacaoDePlanoDeConta;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ServicoDeSugestaoDeCodigo>();
builder.Services.AddControllers();
builder.Services.AddDbContext<AgregacaoDeContaDbContext>(options => options.UseInMemoryDatabase("AgregacaoDeContaDbContext"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
