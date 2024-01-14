using DemoToListBE.Data.Authentication;
using DemoToListBE.Dto;
using DemoToListBE.Dto.Requests.Data;
using DemoToListBE.Dto.Responses;
using DemoToListBE.Logic.Service.AppService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace DemoToListBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoListController : ControllerBase
    {
        //Get ToDoList Data based on User
        //check 
        IApplicationToDoListService _applicationToDoListService;
        UserManager<ApplicationUser> _userManager;

        public ToDoListController(IApplicationToDoListService applicationToDoListService, UserManager<ApplicationUser> userManager)
        {
            _applicationToDoListService = applicationToDoListService;
            _userManager = userManager;

        }
        [HttpGet]
        public async Task<IActionResult> GetToDoList()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserId = currentUser.Id;
            ToDoListResponseDto toDoListResponseDto = await _applicationToDoListService.GetToDoListAsync(currentUserId);
            return Ok(toDoListResponseDto);


        }
        [HttpPost]
        public async Task<IActionResult> PostToDoList(ToDoListRequestDto toDoListRequestDto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                ToDoListDto toDoListDto = new ToDoListDto();
                var currentUserId = currentUser.Id;
                toDoListDto.ApplicationUserId = currentUserId;

                toDoListDto.ToDoListData = toDoListRequestDto.ToDoListData != null ? toDoListRequestDto.ToDoListData.Value.ToString() : string.Empty;


                await _applicationToDoListService.UpdateToDoListAsync(toDoListDto);
                return Ok();
            }
            return BadRequest("Invalid User");

        }
    }
}
