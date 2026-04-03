using ApiCatalago.Models;
using AutoMapper;

namespace ApiCatalago.DTO.Mappings
{
    public class ProdutoDTOMappingProfile : Profile
    {
        public ProdutoDTOMappingProfile()
        {
            CreateMap<Produto,ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            //Request e response do produtoDTO
            CreateMap<Produto, ProdutoDTOUpdateRequest>().ReverseMap();
            CreateMap<Produto, ProdutoDTOUpdateResponse>().ReverseMap();

        }
    }
}
