using DemoToListBE.Data.DomainModel;
using DemoToListBE.Data.Entity;

namespace DemoToListBE.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _applicationDbContext;
        public GenericRepository<ToDoList, ToDoListModel> _toDoListRepository;

        public UnitOfWork(ApplicationDbContext DbContext)
        {
            _applicationDbContext = DbContext;
        }
        public GenericRepository<ToDoList, ToDoListModel> ToDoListRepository
        {
            get
            {
                return _toDoListRepository ?? (_toDoListRepository = new GenericRepository<ToDoList, ToDoListModel>(_applicationDbContext));
            }
        }
        public async Task SaveAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
