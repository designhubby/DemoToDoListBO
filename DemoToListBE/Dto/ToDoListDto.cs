using Newtonsoft.Json.Linq;

namespace DemoToListBE.Dto
{
    public record ToDoListDto
    {
        public string ApplicationUserId { get; set; } = string.Empty;
        public string ToDoListData { get; set; }
    }
}
