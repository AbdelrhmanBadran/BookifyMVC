using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Categories
            CreateMap<Category , CategoryViewModel>().ReverseMap();
            CreateMap<Category , CategoryFormViewModel>().ReverseMap();
            CreateMap<Category , SelectListItem>()
                .ForMember(dest => dest.Value , opt => opt.MapFrom(src => src.Id) )
                .ForMember(dest => dest.Text , opt => opt.MapFrom(src => src.Name) );
            //Authors
            CreateMap<Author ,AuthorViewModel>().ReverseMap();
            CreateMap<Author ,AuthorFormViewModel>().ReverseMap();
            CreateMap<Author , SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name)); ;
            //Books
            CreateMap<Book , BookFormViewModel>().ReverseMap();
        }
    }
}
