import { ISpecialty as Specialty } from './specialty';
import { IUser as User } from './user';

export interface IUserSpecialty { 
    userSpecialtyId?: number;
    userId?: number;
    specialityId?: number;
    specialtyOther?: string;
    speciality?: Specialty;
    user?: User;
}
