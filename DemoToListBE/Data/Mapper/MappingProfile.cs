using AutoMapper;
using DemoToListBE.Data.DomainModel;
using DemoToListBE.Dto.Requests.Data;
using DemoToListBE.Dto.Responses;

namespace DemoToListBE.Data.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ToDoListModel, ToDoListResponseDto>()
                .ForMember(dest => dest.ApplicationUserId, o => o.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.ToDoListData, o => o.MapFrom(src => src.ToDoListData));

        }
    }
}
