using DemoToListBE.Data.Authentication;

namespace DemoToListBE.Data.Entity
{
    public class ToDoList : BaseEntity
    {
        public string ToDoListData { get; set; } = String.Empty;
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
