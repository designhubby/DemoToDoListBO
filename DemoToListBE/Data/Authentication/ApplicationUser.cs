using DemoToListBE.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace DemoToListBE.Data.Authentication
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public int? ToDoListId { get; set; }
        public virtual ToDoList ToDoList { get; set; } = null!;
    }
}
