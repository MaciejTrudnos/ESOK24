using AutoMapper;
using esok.api.Data;
using esok.api.DTO;

namespace esok.api.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<Rent, RentDTO>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<ProductDTO, ReportProductDTO>();
            CreateMap<Article, ShortArticleDTO>();
            CreateMap<Article, ArticleDTO>();
        }
    }
}
