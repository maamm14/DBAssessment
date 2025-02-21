namespace UsersTasks.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
