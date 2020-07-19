import { ILoginProfile as LoginProfile } from './loginProfile';
import { IRole as Role } from './role';

export interface ILoginProfileRole { 
    loginProfileRoleId?: number;
    loginProfileId?: number;
    roleId?: number;
    status?: string;
    loginProfile?: LoginProfile;
    role?: Role;
}
