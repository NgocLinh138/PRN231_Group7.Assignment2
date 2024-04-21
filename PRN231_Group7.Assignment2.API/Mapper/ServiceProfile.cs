using AutoMapper;
using PRN231_Group7.Assignment2.Repo.Model;
using BookResponse = PRN231_Group7.Assignment2.Contract.Service.Book.Response;
using PublisherResponse = PRN231_Group7.Assignment2.Contract.Service.Publisher.Response;
using AuthorResponse = PRN231_Group7.Assignment2.Contract.Service.Author.Response.AuthorRepsonse;
using RoleResponse = PRN231_Group7.Assignment2.Contract.Service.Role.Response;
using BookAuthorResponse = PRN231_Group7.Assignment2.Contract.Service.BookAuthor.Response.BookAuthorResponse;
using UserResponse = PRN231_Group7.Assignment2.Contract.Service.User.Response;


namespace PRN231_Group7.Assignment2.API.Mapper
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Book, BookResponse>().ReverseMap();
            CreateMap<Book, BookResponse>().ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
            CreateMap<Author, AuthorResponse>().ReverseMap();
            CreateMap<Role, RoleResponse>().ReverseMap();
            CreateMap<BookAuthor, BookAuthorResponse>().ReverseMap();
            CreateMap<Publisher, PublisherResponse>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();

        }
    }
}
