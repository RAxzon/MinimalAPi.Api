namespace Students.Data
{
    public abstract class BaseEntity
    {
        public int Id { get; init; }
        public DateTime Created {  get; set; }
        
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime Modified { get; set;}

        public string ModifiedBy { get; set;} = string.Empty;
    }
}
