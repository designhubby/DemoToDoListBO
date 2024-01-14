using Newtonsoft.Json.Linq;

namespace DemoToListBE.Dto.Requests.Data
{
    public record ToDoListRequestDto
    {
        public string ApplicationUserId { get; set; } = string.Empty;
        public JRaw? ToDoListData { get; set; }
    }
}
