using Microsoft.AspNetCore.Mvc;

namespace DemoToListBE.ExceptionHandle.CustomProblemDetails
{
    public class ToDoListCustomDetails: ProblemDetails
    {
        public string AdditionalInfo { get; set; }

    }
}
