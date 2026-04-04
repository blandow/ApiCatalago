using ApiCatalago.Models;

namespace ApiCatalago.DTO.Mappings
{
    public static class CategoriaDTOMappingExtentions
    {
        public static CategoriaDTO? toCategoriaDTO(this Categoria categoria) 
        { 
            if (categoria == null)
            {
                return null;
            }
            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };
        }

        public static Categoria? toCategoria(this CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null)
            {
                return null;
            }
            return new Categoria
            {
                Id = categoriaDTO.Id,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl
            };
        }

        public static IEnumerable<CategoriaDTO>? toListCategoriaDTOs(this IEnumerable<Categoria> categorias)
        {
            if (categorias is null ||!categorias.Any())
            {
                return new List<CategoriaDTO>();
            }

            return categorias.Select(cat => new CategoriaDTO
            {
                Id = cat.Id,
                Nome = cat.Nome,
                ImagemUrl = cat.ImagemUrl
            }).ToList(); 
        }
    }
}
