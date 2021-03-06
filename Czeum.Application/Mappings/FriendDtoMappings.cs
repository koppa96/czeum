using AutoMapper;
using Czeum.Core.DTOs.UserManagement;
using Czeum.Domain.Entities;

namespace Czeum.Application.Mappings
{
    public class FriendDtoMappings : Profile
    {
        public FriendDtoMappings()
        {
            CreateMap<FriendRequest, FriendRequestDto>()
                .ForMember(dst => dst.SenderName, cfg => cfg.MapFrom(src => src.Sender.UserName))
                .ForMember(dst => dst.ReceiverName, cfg => cfg.MapFrom(src => src.Receiver.UserName));
        }
    }
}