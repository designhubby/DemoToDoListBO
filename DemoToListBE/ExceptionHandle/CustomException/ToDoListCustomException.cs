namespace DemoToListBE.ExceptionHandle.CustomException
{
    public class ToDoListCustomException : Exception
    {
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Title { get; set; }
        public string AdditionalInfo { get; set; }
        public string Instance { get; set; }
        
        public ToDoListCustomException(string instance)
        {
            Type = "ToDoList-Custom-Exception";
            Detail = "There was an exception error while fetching the ToDoList";
            Title = "ToDoList Custom Exception";
            AdditionalInfo = "ToDoList not working, please try again";
            Instance = instance;
        } 
    }
}
