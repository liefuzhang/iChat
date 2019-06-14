using AutoMapper;
using iChat.Api.Dtos;
using iChat.Api.Models;
using System.Globalization;

namespace iChat.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.TimeString, m => m.MapFrom(src => src.CreatedDate.ToString("h:mm tt", CultureInfo.InvariantCulture)));
            CreateMap<MessageDto, Message>();
            CreateMap<Conversation, ConversationDto>();
            CreateMap<ConversationDto, Conversation>();
            CreateMap<Channel, ChannelDto>();
            CreateMap<ChannelDto, Channel>();
            CreateMap<File, FileDto>();
            CreateMap<FileDto, File>();
        }
    }
}