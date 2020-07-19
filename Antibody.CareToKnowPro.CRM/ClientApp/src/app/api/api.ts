export * from './account.service';
import { AccountService } from './account.service';
export * from './eventEntities.service';
import { EventEntitiesService } from './eventEntities.service';
export * from './eventEntityProperties.service';
import { EventEntityPropertiesService } from './eventEntityProperties.service';
export * from './events.service';
import { EventsService } from './events.service';
export * from './loginProfileRoles.service';
import { LoginProfileRolesService } from './loginProfileRoles.service';
export * from './loginProfiles.service';
import { LoginProfilesService } from './loginProfiles.service';
export * from './roles.service';
import { RolesService } from './roles.service';
export * from './userSpecialties.service';
import { UserSpecialtiesService } from './userSpecialties.service';
export * from './userUnsubscribes.service';
import { UserUnsubscribesService } from './userUnsubscribes.service';
export * from './users.service';
import { UsersService } from './users.service';
export const APIS = [AccountService, EventEntitiesService, EventEntityPropertiesService, EventsService, LoginProfileRolesService, LoginProfilesService, RolesService, UserSpecialtiesService, UserUnsubscribesService, UsersService];
