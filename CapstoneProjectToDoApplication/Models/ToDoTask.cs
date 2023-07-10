using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618

namespace CapstoneProjectToDoApplication.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ?Description { get; set; }
        public string Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created Date")]
        public DateTime Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Completed")]
        public bool ?IsCompleted { get; set; }
        public int ListId { get; set; }
        public ToDoList List { get; set; }
    }
}
