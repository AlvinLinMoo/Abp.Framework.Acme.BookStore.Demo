using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using AutoMapper;
namespace Acme.BookStore.Web;

public class BookStoreWebAutoMapperProfile : Profile
{
    public BookStoreWebAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Web project.
        CreateMap<BookDto, CreateUpdateBookDto>().ReverseMap();

        CreateMap<Pages.Authors.CreateModalModel.CreateAuthorViewModel, CreateAuthorDto>().ReverseMap();
        CreateMap<Pages.Authors.EditModalModel.EditAuthorViewModel, AuthorDto>().ReverseMap();
        CreateMap<Pages.Authors.EditModalModel.EditAuthorViewModel, UpdateAuthorDto>().ReverseMap();

        CreateMap<Pages.Books.CreateModalModel.CreateBookViewModel, CreateUpdateBookDto>().ReverseMap();
        CreateMap<BookDto, Pages.Books.EditModalModel.EditBookViewModel>().ReverseMap();
        CreateMap<Pages.Books.EditModalModel.EditBookViewModel, CreateUpdateBookDto>().ReverseMap();
    }
}
