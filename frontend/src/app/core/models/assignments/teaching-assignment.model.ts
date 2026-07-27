export interface TeachingAssignment {
  id: number;

  schoolClassId: number;
  subjectId: number;
  teacherId: number; // User.id (role = Teacher)
}