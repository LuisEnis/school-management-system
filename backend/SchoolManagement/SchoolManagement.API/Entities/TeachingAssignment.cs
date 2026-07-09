namespace SchoolManagement.API.Entities
{
    public class TeachingAssignment
    {
        public int Id { get; set; }


        public int TeacherId { get; set; }

        public User Teacher { get; set; } = null!;


        public int SubjectId { get; set; }

        public Subject Subject { get; set; } = null!;


        public int SchoolClassId { get; set; }

        public SchoolClass SchoolClass { get; set; } = null!;
    }
}
