import { UserDto } from '../users/user.dto';
import { StudentSubjectDto } from '../users/student-subject.dto';


export interface ClassDetailsDto {

    id:number;

    name:string;

    students:UserDto[];

    subjects:StudentSubjectDto[];

}