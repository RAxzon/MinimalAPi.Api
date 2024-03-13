namespace Students.Data
{
    public abstract class BaseEntity
    {
        public int Id { get; init; }
        public DateTime Created {  get; set; }
        
        public string CreatedBy { get; set; }

        public DateTime Modified { get; set;}

        public string ModifiedBy { get; set;}
    }
}
