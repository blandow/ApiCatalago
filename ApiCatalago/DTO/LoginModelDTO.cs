using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.DTO
{
    public class LoginModelDTO
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string? Password { get; set; }
    }
}
