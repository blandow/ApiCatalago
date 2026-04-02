using ApiCatalago.Validations;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.DTO
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? ImagemUrl { get; set; }

    }
}
