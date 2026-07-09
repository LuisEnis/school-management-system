namespace SchoolManagement.API.Entities
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;


        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

        public ICollection<TeachingAssignment> TeachingAssignments { get; set; } = new List<TeachingAssignment>();
    }
}
