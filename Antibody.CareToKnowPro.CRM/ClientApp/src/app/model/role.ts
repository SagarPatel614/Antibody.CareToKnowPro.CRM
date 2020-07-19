import { ILoginProfileRole as LoginProfileRole } from './loginProfileRole';

export interface IRole { 
    roleId?: number;
    name: string;
    description?: string;
    permission?: string;
    loginProfileRole?: Array<LoginProfileRole>;
}
