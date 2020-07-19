import { ISpecialtyGroup as SpecialtyGroup } from './specialtyGroup';
import { IUserSpecialty as UserSpecialty } from './userSpecialty';

export interface ISpecialty { 
    specialtyId?: number;
    specialtyNameEn?: string;
    specialtyNameFr?: string;
    position?: number;
    specialtyGroupId?: number;
    specialtyGroup?: SpecialtyGroup;
    userSpecialty?: Array<UserSpecialty>;
}
