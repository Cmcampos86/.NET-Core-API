using ApiFuncional.Data;
using ApiFuncional.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        //desabilita o filtro de erros que é mostrado quando um método da api contem erros. Se desabilitar, a responsabilidate de validação fica a cargo do desenvolvedor
        options.SuppressModelStateInvalidFilter = true; 
    });

builder.Services.AddEndpointsApiExplorer();

//Configura o Swagger para que possa usar o JWT (Aparece um botão chamado Authorize com um cadeado)
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//Configuração do banco de dados
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Configuração do Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>() //IdentityUser representa o usuário logado e IdentityRole representa o perfil
                .AddRoles<IdentityRole>() //Adiciona Perfis
                .AddEntityFrameworkStores<ApiDbContext>(); //Mecanismo de store e para qual contexto de Banco de Dados

// Pegando o Token e gerando a chave encodada
var JwtSettingsSection = builder.Configuration.GetSection("JwtSettings"); //Configuração: vai pegar os dados que stão na appSettings.json
builder.Services.Configure<JwtSettings>(JwtSettingsSection);

var jwtSettings = JwtSettingsSection.Get<JwtSettings>(); //Vai pegar a instância da classe populada
var key = Encoding.ASCII.GetBytes(jwtSettings.Segredo); //Vai criar a chave em uma sequencia de bytes

//Configura a autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Esquema padrão de autenticação
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //Verifica se o Token é válido (um desafio... challenge)
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; //Trabalhar em um Https
    options.SaveToken = true; //Permitir que o Token seja salvo caso esteja com sucesso
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(key), //Chave usada para emissão do Token
        ValidateIssuer = true, //Validar o emissor
        ValidateAudience = true, //Validar se é compatível com a audiência do Token
        ValidAudience = jwtSettings.Audiencia, //Audiência da configuração
        ValidIssuer = jwtSettings.Emissor  //Emissor da configuração
    };
});

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
