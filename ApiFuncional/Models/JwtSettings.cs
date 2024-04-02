namespace ApiFuncional.Models
{
    public class JwtSettings
    {
        public string? Segredo { get; set; } //Vai assinar o token
        public int ExpiracaoHoras { get; set; } 
        public string? Emissor { get; set; } //Qual a aplicação que esta emitindo o token
        public string? Audiencia { get; set; } //Em que lugar o token é válido
    }
}
