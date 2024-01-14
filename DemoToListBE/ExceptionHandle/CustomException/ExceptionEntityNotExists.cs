namespace DemoToListBE.ExceptionHandle.CustomException
{
    public class ExceptionEntityNotExists : Exception
    {
        public ExceptionEntityNotExists(string message): base($"Entity {message} ")
        {

        }
        public ExceptionEntityNotExists(string message, Exception inner): base(message, inner)
        {


        }
    }
}
