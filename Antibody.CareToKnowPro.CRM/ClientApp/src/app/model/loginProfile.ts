import { IEvent as Event } from './event';
import { ILoginProfileRole as LoginProfileRole } from './loginProfileRole';

export interface ILoginProfile {
    //password: string;
    loginProfileId?: number;
    email: string;
    emailConfirmed?: boolean;
    userName: string;
    passwordHash?: string;
    lockoutEndDateUtc?: Date;
    lockoutEnabled?: boolean;
    accessFailedCount?: number;
    companyName?: string;
    firstName?: string;
    lastName?: string;
    street1?: string;
    city?: string;
    provCode?: string;
    postal?: string;
    country?: string;
    phoneNumber?: string;
    notes?: string;
    status: boolean;
    profileQuestion?: string;
    profileAnswer?: string;
    event?: Array<Event>;
    loginProfileRole?: Array<LoginProfileRole>;
}

export interface IPassword {
  oldPassword: string;
  newPassword: string;
  confirmNewPassword: string;
  loginProfileId: number;
}
