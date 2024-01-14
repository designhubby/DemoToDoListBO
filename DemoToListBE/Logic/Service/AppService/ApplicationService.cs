using AutoMapper;
using DemoToListBE.Data.Authentication;
using DemoToListBE.Data.Entity;
using DemoToListBE.Data.Repository;
using DemoToListBE.Logic.Service.ModelService;
using Microsoft.AspNetCore.Identity;

namespace DemoToListBE.Logic.Service.AppService
{
    public partial class ApplicationService : IApplicationToDoListService
    {
        IUnitOfWork _uow;
        IMapper _mapper;
        ToDoListService _toDoListService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationService(IUnitOfWork uow, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _uow = uow;
            _mapper = mapper;
            _toDoListService = new ToDoListService(_uow);
            _userManager = userManager;

        }

    }
}
