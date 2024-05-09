using AutoMapper;
using Server.API.DTO;

namespace Server.API.Mapping;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<Chat, ChatDTO>();
        CreateMap<Message, MessageDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User == null ? "Deleted" : src.User.Name));
        CreateMap<UserDTO, User>();
        CreateMap<ChatDTO, Chat>();
        CreateMap<MessageDTO, Message>();
    }
}
