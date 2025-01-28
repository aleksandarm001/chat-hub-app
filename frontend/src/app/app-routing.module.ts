import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Chat2Component } from './features/chat-2/chat-2.component';
import { LoginComponent } from './infrastructure/auth/login/login.component';

const routes: Routes = [
  {path: 'chat2', component: Chat2Component},
  {path: 'login', component: LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }