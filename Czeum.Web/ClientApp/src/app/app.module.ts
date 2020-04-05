import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { reducers } from './reducers';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthenticationModule } from './authentication/authentication.module';
import { storageSyncMetaReducer } from 'ngrx-store-persist';
import { API_BASE_URL } from './shared/clients';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule,
    HttpClientModule,
    StoreModule.forRoot(reducers, { metaReducers: [ storageSyncMetaReducer ] }),
    AuthenticationModule
  ],
  providers: [
    HttpClient,
    { provide: API_BASE_URL, useValue: 'https://localhost:5001' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
