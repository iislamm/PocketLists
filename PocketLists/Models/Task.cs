namespace PocketLists.Models
{
    public class Task
    {
        public int TaskID { get; set; }
        public string TaskTitle { get; set; }
        public int TaskOwner{ get; set; }
        public int Done { get; set; }
    }
}
