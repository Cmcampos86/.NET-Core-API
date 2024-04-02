using ApiFuncional.Data;
using ApiFuncional.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiFuncional.Configuration
{
    public static class IdentityConfig
    {
        public static WebApplicationBuilder AddIdentityConfig(this WebApplicationBuilder builder)
        {
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

            return builder;
        }
    }
}
