export interface TeachingAssignment {
  id: number;

  classId: number;
  subjectId: number;
  teacherId: number; // User.id (role = Teacher)
}