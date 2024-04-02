using ApiFuncional.Data;
using Microsoft.AspNetCore.Identity;
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

//Configuração do Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>() //IdentityUser representa o usuário logado e IdentityRole representa o perfil
                .AddRoles<IdentityRole>() //Adiciona Perfis
                .AddEntityFrameworkStores<ApiDbContext>(); //Mecanismo de store e para qual contexto de Banco de Dados

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
