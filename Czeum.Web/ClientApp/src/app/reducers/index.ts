import { ActionReducerMap } from '@ngrx/store';
import { FriendListItem } from '../shared/models/friend-list.models';
import { friendListReducer } from './friend-list/friend-list-reducers';
import { soloQueueReducer } from './solo-queue/solo-queue-reducers';
import { Profile } from '../authentication/auth-config-models';
import { authStateReducer, pkcsStringReducer } from './authentication/auth-reducers';

export interface AuthState {
  isAuthenticated: boolean;
  profile: Profile | null;
  accessToken: string | null;
  idToken: string | null;
  expires: Date | null;
}

export interface State {
  friendList: FriendListItem[];
  isQueueing: boolean;
  authState: AuthState;
  pkceString: string;
}

export const reducers: ActionReducerMap<State> = {
  friendList: friendListReducer,
  isQueueing: soloQueueReducer,
  authState: authStateReducer,
  pkceString: pkcsStringReducer
};
