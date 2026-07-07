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
        path: 'assignments',
        component: TeacherSubjectAssignment
      }
    ]
  }
];