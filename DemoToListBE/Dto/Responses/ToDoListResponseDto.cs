using Newtonsoft.Json.Linq;

namespace DemoToListBE.Dto.Responses
{
    public record ToDoListResponseDto
    {
        public string ApplicationUserId { get; set; }   = string.Empty;
        public JRaw ToDoListData { get; set; } = new JRaw(string.Empty);
    }
}
