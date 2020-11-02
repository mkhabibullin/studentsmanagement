namespace SM.Domain.Entities
{
    public class StudentGroup
    {
        public long StudentId { get; set; }
        public Student Student { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
