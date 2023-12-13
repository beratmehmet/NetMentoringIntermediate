using AutoMapper;
using RESTArchitecture.Models.Categories;

namespace RESTArchitecture.Models.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryForCreate, Category>();
        }
    }
}
