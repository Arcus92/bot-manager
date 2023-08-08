import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import {TypeObjectComponent} from "./components/type-object/type-object.component";
import {TypeListComponent} from "./components/type-list/type-list.component";
import {TypeEditorComponent} from "./components/type-editor/type-editor.component";

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    TypeEditorComponent,
    TypeObjectComponent,
    TypeListComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
