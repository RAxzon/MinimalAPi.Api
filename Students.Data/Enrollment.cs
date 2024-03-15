namespace Models.Data
{
    public class Enrollment : BaseEntity
    {
        public int CourseId { get; set; }
        public int StudentID { get; set; }
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}
