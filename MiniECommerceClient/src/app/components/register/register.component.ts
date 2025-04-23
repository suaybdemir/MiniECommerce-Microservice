import { Component, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserModel } from '../../models/user.model';
import {FormsModule} from '@angular/forms'
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { api } from '../../constants';
import { ResultModel } from '../../models/result.model';
import {FlexiToastService} from 'flexi-toast'

@Component({
  selector: 'app-register',
  standalone:true,
  imports: [RouterLink,FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  model = signal<UserModel>(new UserModel())

  constructor(
    private http : HttpClient,
    private toast: FlexiToastService,
    private router:Router
  ){}

  register()
  {
    this.http.post<ResultModel<string>>(`${api}/auth/register`,this.model()).subscribe({
      next:(res)=>{
        this.toast.showToast("Successfull",res.data!,"success")
        this.router.navigateByUrl("/login")
      },
      error:(err:HttpErrorResponse)=>{
        console.log(err);
        this.toast.showToast("Error!","Something went wrong!","error")
      }
    })
  }
}
