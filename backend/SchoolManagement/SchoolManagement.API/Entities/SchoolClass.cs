namespace SchoolManagement.API.Entities
{
    public class SchoolClass
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;


        public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();

        public ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();
    }
}
