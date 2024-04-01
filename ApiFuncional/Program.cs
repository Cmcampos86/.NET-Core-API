using ApiFuncional.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        //desabilita o filtro de erros que é mostrado quando um método da api contem erros. Se desabilitar, a responsabilidate de validação fica a cargo do desenvolvedor
        options.SuppressModelStateInvalidFilter = true; 
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuração do banco de dados
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
