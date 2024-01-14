using DemoToListBE.Data.DomainModel;
using DemoToListBE.Data.Entity;

namespace DemoToListBE.Data.Repository
{
    public interface IUnitOfWork
    {
        GenericRepository<ToDoList, ToDoListModel> ToDoListRepository { get; }

        Task SaveAsync();
    }
}