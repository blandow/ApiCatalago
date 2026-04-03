using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.DTO
{
    public class ProdutoDTOUpdateRequest:IValidatableObject
    {
        [Range(1,9999999999,ErrorMessage = "Estoque deve ser entre 1 e 9999999999 ")]
        public float Estoque { get; set; }
        public DateTime ? DataCadastro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataCadastro.Value <= DateTime.Now.Date && DataCadastro != DateTime.MinValue)
            {
                yield return
                    new ValidationResult("A Data deve ser maior que a data atual", new[] { nameof(this.DataCadastro) });
            }
        }
    }
}
