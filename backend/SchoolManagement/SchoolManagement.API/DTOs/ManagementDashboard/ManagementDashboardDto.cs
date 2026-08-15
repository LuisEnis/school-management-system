namespace SchoolManagement.API.DTOs.ManagementDashboard
{
    public class ManagementDashboardDto
    {
        public int TotalStudents { get; set; }

        public int TotalTeachers { get; set; }

        public int TotalClasses { get; set; }

        public int TotalSubjects { get; set; }

        public int TotalStudentClassAssignments { get; set; }

        public int TotalTeacherSubjectAssignments { get; set; }

        public int TotalTeachingAssignments { get; set; }
    }
}
