using DemoToListBE.Data.Authentication;
using DemoToListBE.Data.DomainModel;
using DemoToListBE.Data.Repository;
using DemoToListBE.Dto;
using DemoToListBE.Dto.Requests.Data;
using DemoToListBE.Dto.Responses;
using Newtonsoft.Json.Linq;

namespace DemoToListBE.Logic.Service.ModelService
{
    public class ToDoListService
    {
        IUnitOfWork _unitOfWork;
        public ToDoListService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Dto -> Model -> Update / Add?
        //get entity root to do stuff
        public async Task<bool> CheckEntityExistsByApplicationId(string applicationUserId)
        {
            bool exists = await _unitOfWork.ToDoListRepository.CheckEntityExistsWhere(t => t.ApplicationUserId == applicationUserId);
            return exists;
        }
        public async Task UpdateToDoList(string toDoListData, string applicationUserId)
        {
            bool exists = await _unitOfWork.ToDoListRepository.CheckEntityExistsWhere(t => t.ApplicationUserId == applicationUserId);
            if(!exists)
            {
                await CreateToDoList(applicationUserId, toDoListData);
            }
            else
            {
                ToDoListModel toDoListModel = await _unitOfWork.ToDoListRepository.GetDomainModelWhereFirstAsync(t => t.ApplicationUserId == applicationUserId);
                toDoListModel.UpdateToDoListData(toDoListData);
                await _unitOfWork.SaveAsync();
            }
        }
        public async Task UpdateToDoList(ToDoListDto toDoListDto)
        {
            await UpdateToDoList(toDoListDto.ToDoListData, toDoListDto.ApplicationUserId);
        }
        public async Task<ToDoListResponseDto> GetToDoList(string applicationUserId)
        {
            bool exists = await _unitOfWork.ToDoListRepository.CheckEntityExistsWhere(t => t.ApplicationUserId == applicationUserId);
            if (!exists)
            {
                return null;
            }
            ToDoListModel toDoListModel = await _unitOfWork.ToDoListRepository.GetDomainModelWhereFirstAsync(t => t.ApplicationUserId == applicationUserId);
            ToDoListResponseDto toDoListResponseDto = new ToDoListResponseDto()
            {
                ToDoListData = new JRaw(toDoListModel.ToDoListData),
                ApplicationUserId = applicationUserId,
            };
            return toDoListResponseDto;
        }

        public async Task<ToDoListModel> CreateToDoList(string applicationUserId, string toDoListData)
        {
            ToDoListModel toDoListModel = new ToDoListModel(applicationUserId, toDoListData);
            await _unitOfWork.ToDoListRepository.AddDomainAsync(toDoListModel);

            await _unitOfWork.SaveAsync();
            return toDoListModel;
        }
        public async Task<ToDoListModel> CreateToDoList(string toDoListData, ApplicationUser applicationUser)
        {
            ToDoListModel toDoListModel = new ToDoListModel(applicationUser, toDoListData);
            await _unitOfWork.ToDoListRepository.AddDomainAsync(toDoListModel);

            await _unitOfWork.SaveAsync();
            return toDoListModel;
        }
    }
}
