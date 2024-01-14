using DemoToListBE.Data.Authentication;
using DemoToListBE.Data.Entity;

namespace DemoToListBE.Data.DomainModel
{
    public class ToDoListModel : BaseDomainModel<ToDoList>
    {

        public ToDoListModel(ToDoList _toDoList): base(_toDoList)
        {
           
        }
        public ToDoListModel(string applicationUserId, string _listdata)
        {
            _entity = new ToDoList()
            {
                ApplicationUserId = applicationUserId,
                ToDoListData = _listdata,
            };
        }
        public ToDoListModel(ApplicationUser applicationUser, string _listdata)
        {
            _entity = new ToDoList()
            {
                ApplicationUser = applicationUser,
                ToDoListData = _listdata,
            };
        }

        public ToDoListModel(string _listdata)
        {
            _entity = new ToDoList()
            {
                ToDoListData = _listdata,
            };
        }
        public string ToDoListData 
        { 
            get => _entity.ToDoListData;
            set => _entity.ToDoListData = value;
        }
        public int ToDoListId
        {
            get => _entity.Id;
        }
        public ApplicationUser ApplicationUser
        {
            get => _entity.ApplicationUser;
            set => _entity.ApplicationUser = value;
        }
        public string ApplicationUserId
        {
            get => _entity.ApplicationUserId;
            set => _entity.ApplicationUserId = value;
        }
        //Add to ToDoList
        //Pretend this is the shopping cart entity
        //Modify data
        public void UpdateToDoListData(string toDoData)
        {
            _entity.ToDoListData = toDoData;
        }
        public void DeleteToDoListData()
        {
            _entity.ToDoListData = "";
        }

    }
}
