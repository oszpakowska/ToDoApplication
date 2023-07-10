#pragma warning disable CS8618

namespace CapstoneProjectToDoApplication.Models
{
    public class ToDoList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool ?Hide { get; set; }
        public ICollection<ToDoTask> Tasks { get; set; }
    }
}
