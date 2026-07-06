import { Routes } from '@angular/router';
import { MainLayout } from './layout/main-layout/main-layout';
import { Login } from './features/auth/login/login';
import { authGuard } from './core/guards/auth.guard';
import { Dashboard } from './features/dashboard/dashboard';
import { StudentList } from './features/students/student-list/student-list';
import { StudentForm } from './features/students/student-form/student-form';
import { TeacherList } from './features/teachers/teacher-list/teacher-list';
import { TeacherForm } from './features/teachers/teacher-form/teacher-form';

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
      }
    ]
  }
];