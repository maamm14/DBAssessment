namespace UsersTasks.Models.DTO
{
    public class CreateTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
    }

    public class UpdateTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
    }

}
