using ApiFuncional.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApiConfig()
    .AddCorsConfig()
    .AddSwaggerConfig()
    .AddDbContextConfig()
    .AddIdentityConfig();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Development"); //Aponta o perfil CORS de Desenvolvimento
}
else
{
    app.UseCors("Production"); //Aponta o perfil CORS de Produção
}

app.UseHttpsRedirection(); //Utiliza HTTPS

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
