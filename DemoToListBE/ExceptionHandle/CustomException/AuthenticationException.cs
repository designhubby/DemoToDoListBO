namespace DemoToListBE.ExceptionHandle.CustomException
{
    public class AuthenticationException: Exception
    {
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Title { get; set; }
        public string Instance { get; set; }

        public AuthenticationException(string instance)
        {
            Type = "Authentication-Exception";
            Detail = "There was an exception error while Authenticating ToDoList";
            Title = "ToDoList Authentication-Exception";
            Instance = instance;
        }
    }
}
