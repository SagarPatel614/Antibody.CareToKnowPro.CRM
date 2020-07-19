export * from './alert.service';
import { AlertService } from './alert.service';
export * from './auth-guard.service';
import { AuthGuard } from './auth-guard.service';
export * from './auth.service';
import { AuthService } from './auth.service';

export const Services = [ AlertService, AuthService, AuthGuard];
