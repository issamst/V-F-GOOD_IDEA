import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users/users.component';
import { CommitteesComponent  } from './committees/committees.component';
import { TeamLeaderComponent } from './team-leader/team-leader.component';


const routes: Routes = [

  {path:'users',component:UsersComponent},
  {path:'committees',component:CommitteesComponent},
  {path:'team-leader',component:TeamLeaderComponent},


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManagerResourcesRoutingModule { }
