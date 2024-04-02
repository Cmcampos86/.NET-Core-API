namespace ApiFuncional.Configuration
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddApiConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    //desabilita o filtro de erros que é mostrado quando um método da api contem erros. Se desabilitar, a responsabilidate de validação fica a cargo do desenvolvedor
                    options.SuppressModelStateInvalidFilter = true;
                });

            return builder;
        }
    }
}
