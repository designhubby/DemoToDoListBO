using DemoToListBE.Data.Authentication;
using DemoToListBE.Data.DomainModel;
using DemoToListBE.Data.Entity;
using DemoToListBE.Dto;
using DemoToListBE.Dto.Requests.Data;
using DemoToListBE.Dto.Responses;
using DemoToListBE.Logic.Service.ModelService;

namespace DemoToListBE.Logic.Service.AppService
{
    public partial class ApplicationService
    {

        public async Task<ToDoListResponseDto> GetToDoListAsync(string applicationUserId)
        {
            ToDoListResponseDto toDoListResponseDto = await _toDoListService.GetToDoList(applicationUserId);
            return toDoListResponseDto;
        }


        public async Task UpdateToDoListAsync(ToDoListDto toDoListDto)
        {
            //check if it exists
            bool exists = await _toDoListService.CheckEntityExistsByApplicationId(toDoListDto.ApplicationUserId);
            ///if not, 
            ////create ToDoList with applicationUserId
            ///update applicationUser with ToDoListId
            ///if so, update
            ///
            if (!exists)
            {

                ApplicationUser _applicationUser = await _userManager.FindByIdAsync(toDoListDto.ApplicationUserId);
                await _toDoListService.CreateToDoList(toDoListDto.ToDoListData, _applicationUser);

            }
            else
            {
                await _toDoListService.UpdateToDoList(toDoListDto);
            }

        }

    }
}
