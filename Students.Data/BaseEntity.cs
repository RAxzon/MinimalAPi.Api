namespace Models.Data
{
    public abstract class BaseEntity
    {
        public int Id { get; init; }
        public DateTime Created {  get; set; } = DateTime.Now;
        
        public string CreatedBy { get; set; } = "User";

        public DateTime Modified { get; set;} = DateTime.Now;

        public string ModifiedBy { get; set;} = "User";
    }
}
