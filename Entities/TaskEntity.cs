using SQLite;

namespace Task.Management.Entities
{
    [Table("Tasks")]
    public class TaskEntity
    {
        [PrimaryKey]    
        public string TaskId { get; set; }
        [NotNull]
        public string Title { get; set; }
        public string Description { get; set; } 
        public DateTime DueDate { get; set; }
    }
}
