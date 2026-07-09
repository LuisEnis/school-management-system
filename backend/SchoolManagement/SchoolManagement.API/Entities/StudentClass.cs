namespace SchoolManagement.API.Entities
{
    public class StudentClass
    {
        public int Id { get; set; }


        public int StudentId { get; set; }

        public User Student { get; set; } = null!;


        public int SchoolClassId { get; set; }

        public SchoolClass SchoolClass { get; set; } = null!;
    }
}
