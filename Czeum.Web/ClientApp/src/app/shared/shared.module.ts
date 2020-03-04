import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MenuComponent } from './components/menu/menu.component';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { NotificationComponent } from './components/notification/notification.component';
import { MatCardModule } from '@angular/material/card';
import { FlexModule } from '@angular/flex-layout';
import { FriendsListComponent } from './components/friends-list/friends-list.component';
import { NotificationListItemComponent } from './components/notification-list-item/notification-list-item.component';

@NgModule({
  declarations: [
    MenuComponent,
    NotificationComponent,
    FriendsListComponent,
    NotificationListItemComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatInputModule,
    MatListModule,
    MatSelectModule,
    MatIconModule,
    MatCardModule,
    RouterModule,
    FlexModule
  ],
  exports: [
    MatButtonModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatInputModule,
    MatListModule,
    MatSelectModule,
    MenuComponent,
    NotificationComponent,
    MatIconModule,
    MatCardModule,
    FlexModule,
    FriendsListComponent
  ]
})
export class SharedModule { }
