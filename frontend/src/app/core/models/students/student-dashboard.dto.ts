import { StudentSubjectDto } from '../users/student-subject.dto';


export interface StudentDashboardDto {

    classId:number;

    className:string;

    subjects:StudentSubjectDto[];

}