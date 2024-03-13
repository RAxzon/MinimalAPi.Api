using Students.Data;

namespace Models.Data
{
    public class Course : BaseEntity
    {
        public string Title { get; set; }
        public int Credits { get; set; }
    }
}
