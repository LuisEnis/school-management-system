import { Routes } from '@angular/router';
import { MainLayout } from './layout/main-layout/main-layout';
import { Login } from './features/auth/login/login';
import { authGuard } from './core/guards/auth.guard';
import { managementGuard } from './core/guards/management.guard';
import { directorGuard } from './core/guards/director.guard';
import { classViewGuard } from './core/guards/class-view.guard';
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
import { SecretaryList } from './features/secretaries/secretary-list/secretary-list';
import { SecretaryForm } from './features/secretaries/secretary-form/secretary-form';
import { Profile } from './features/auth/profile/profile';

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
        path: 'profile',
        component: Profile,
        canActivate: [directorGuard]
      },
      {
        path: 'change-password',
        component: ChangePassword
      },
      {
      path: 'students',
        component: StudentList,
        canActivate: [managementGuard]
      },
      {
        path: 'students/new',
        component: StudentForm,
        canActivate: [managementGuard]
      },
      {
        path: 'students/edit/:id',
        component: StudentForm,
        canActivate: [managementGuard]
      },
      {
        path: 'teachers',
        component: TeacherList,
        canActivate: [managementGuard]
      },
      {
        path: 'teachers/new',
        component: TeacherForm,
        canActivate: [directorGuard]
      },
      {
        path: 'teachers/edit/:id',
        component: TeacherForm,
        canActivate: [directorGuard]
      },
      {
        path: 'subjects',
        component: SubjectList,
        canActivate: [managementGuard]
      },
      {
        path: 'subjects/new',
        component: SubjectForm,
        canActivate: [managementGuard]
      },
      {
        path: 'subjects/edit/:id',
        component: SubjectForm,
        canActivate: [managementGuard]
      },
      {
        path: 'schoolClasses',
        component: SchoolClassList,
        canActivate: [managementGuard]
      },
      {
        path: 'schoolClasses/new',
        component: SchoolClassForm,
        canActivate: [managementGuard]
      },
      {
        path: 'schoolClasses/edit/:id',
        component: SchoolClassForm,
        canActivate: [managementGuard]
      },
      {
        path: 'assignments/student-class',
        component: StudentClassAssignment,
        canActivate: [managementGuard]
      },
      {
        path: 'assignments/teacher-subject',
        component: TeacherSubjectAssignment,
        canActivate: [managementGuard]
      },
      {
        path: 'assignments/teaching-assignment',
        component: TeachingAssignment,
        canActivate: [managementGuard]
      },
      {
        path: 'assignments/student-class/new',
        component: StudentClassAssignmentForm,
        canActivate: [managementGuard]
      },
      {
        path: 'assignments/teacher-subject/new',
        component: TeacherSubjectAssignmentForm,
        canActivate: [managementGuard]
      },
      {
        path: 'assignments/teaching-assignment/new',
        component: TeachingAssignmentForm,
        canActivate: [managementGuard]
      },
      {
        path:'schoolClasses/details/:id',
        component:ClassDetails,
        canActivate: [classViewGuard]
      },
      {
        path: 'secretaries',
        component: SecretaryList,
        canActivate: [directorGuard]
      },
      {
        path: 'secretaries/new',
        component: SecretaryForm,
        canActivate: [directorGuard]
      },
      {
        path: 'secretaries/edit/:id',
        component: SecretaryForm,
        canActivate: [directorGuard]
      }
    ]
  }
];