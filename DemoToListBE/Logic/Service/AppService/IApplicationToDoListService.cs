using DemoToListBE.Data.Entity;
using DemoToListBE.Dto;
using DemoToListBE.Dto.Requests.Data;
using DemoToListBE.Dto.Responses;

namespace DemoToListBE.Logic.Service.AppService
{
    public interface IApplicationToDoListService
    {
        Task<ToDoListResponseDto> GetToDoListAsync(string applicationId);
        Task UpdateToDoListAsync(ToDoListDto toDoListRequestDto);
    }
}