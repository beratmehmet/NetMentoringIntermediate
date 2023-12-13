using AutoMapper;
using RESTArchitecture.Models.Items;

namespace RESTArchitecture.Models.Mappers
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<ItemForCreate, Item>();
        }
    }
}
