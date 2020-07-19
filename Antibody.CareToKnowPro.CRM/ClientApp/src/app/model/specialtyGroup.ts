import { ISpecialty as Specialty } from './specialty';

export interface ISpecialtyGroup { 
    specialtyGroupId?: number;
    groupNameEn?: string;
    groupNameFr?: string;
    position?: number;
    specialty?: Array<Specialty>;
}
