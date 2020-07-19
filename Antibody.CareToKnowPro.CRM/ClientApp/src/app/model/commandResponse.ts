
import { ILoginProfile as LoginProfile } from './loginProfile';

export interface ILogin { 
    user?: LoginProfile;
    isAuthenticated?: boolean;
}
