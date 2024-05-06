﻿using AutoMapper;
using Server.API.DTO;

namespace Server.API.Mapping;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<Chat, ChatDTO>();
        CreateMap<Message, MessageDTO>();
    }
}