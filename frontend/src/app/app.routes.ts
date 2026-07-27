import { Routes } from '@angular/router';
import { MainLayout } from './layout/main-layout/main-layout';
import { Login } from './features/auth/login/login';
import { authGuard } from './core/guards/auth.guard';
import { Dashboard } from './features/dashboard/dashboard';
import { StudentList } from './features/students/student-list/student-list';
import { StudentForm } from './features/students/student-form/student-form';
import { TeacherList } from './features/teachers/teacher-list/teacher-list';
import { TeacherForm } from './features/teachers/teacher-form/teacher-form';
import { SubjectList } from './features/subjects/subject-list/subject-list';
import { SubjectForm } from './features/subjects/subject-form/subject-form';
import { SchoolClassList } from './features/schoolClasses/school-class-list/school-class-list';
import { SchoolClassForm } from './features/schoolClasses/school-class-form/school-class-form';
import { TeacherSubjectAssignment } from './features/assignments/teacher-subject-assignment/teacher-subject-assignment';
import { ClassDetails } from './features/schoolClasses/class-details/class-details';
import { ChangePassword } from './features/auth/change-password/change-password';
import { StudentClassAssignmentForm } from './features/assignments/student-class-assignment-form/student-class-assignment-form';
import { TeacherSubjectAssignmentForm } from './features/assignments/teacher-subject-assignment-form/teacher-subject-assignment-form';
import { TeachingAssignmentForm } from './features/assignments/teaching-assignment-form/teaching-assignment-form';
import { StudentClassAssignment } from './features/assignments/student-class-assignment/student-class-assignment';
import { TeachingAssignment } from './features/assignments/teaching-assignment/teaching-assignment';

export const routes: Routes = [
  {
    path: 'login',
    component: Login
  },
  {
    path: '',
    component: MainLayout,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        component: Dashboard
      },
      {
        path: 'change-password',
        component: ChangePassword
      },
      {
      path: 'students',
        component: StudentList
      },
      {
        path: 'students/new',
        component: StudentForm
      },
      {
        path: 'students/edit/:id',
        component: StudentForm
      },
      {
        path: 'teachers',
        component: TeacherList
      },
      {
        path: 'teachers/new',
        component: TeacherForm
      },
      {
        path: 'teachers/edit/:id',
        component: TeacherForm
      },
      {
        path: 'subjects',
        component: SubjectList
      },
      {
        path: 'subjects/new',
        component: SubjectForm
      },
      {
        path: 'subjects/edit/:id',
        component: SubjectForm
      },
      {
        path: 'schoolClasses',
        component: SchoolClassList
      },
      {
        path: 'schoolClasses/new',
        component: SchoolClassForm
      },
      {
        path: 'schoolClasses/edit/:id',
        component: SchoolClassForm
      },
      {
        path: 'assignments/student-class',
        component: StudentClassAssignment
      },
      {
        path: 'assignments/teacher-subject',
        component: TeacherSubjectAssignment
      },
      {
        path: 'assignments/teaching-assignment',
        component: TeachingAssignment
      },
      {
        path: 'assignments/student-class/new',
        component: StudentClassAssignmentForm
      },
      {
        path: 'assignments/teacher-subject/new',
        component: TeacherSubjectAssignmentForm
      },
      {
        path: 'assignments/teaching/new',
        component: TeachingAssignmentForm
      },
      {
        path:'schoolClasses/details/:id',
        component:ClassDetails
      }
    ]
  }
];