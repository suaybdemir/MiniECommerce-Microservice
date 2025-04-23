import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { api } from '../constants';
import { ResultModel } from '../models/result.model';
import { ShoppingCartModel } from '../models/shopping-cart.model';
import { FlexiToastService } from 'flexi-toast';

@Injectable({
  providedIn: 'root'
})
export class CartsService {
  token = signal<string>("");
  carts = signal<ShoppingCartModel[]>([]);

  constructor(
    private http:HttpClient,
    private toast : FlexiToastService
  ){
      if(localStorage.getItem("my-token"))
      {
        this.token.set(localStorage.getItem("my-token")!);
  
        this.getAll();

      }
    }

    getAll() {
      this.http.get<ResultModel<ShoppingCartModel[]>>(`${api}/shoppingCarts/getall`, {
        headers: {
          "Authorization": "Bearer " + this.token()
        }
      }).subscribe((res) => {
        if (res.data) {
          this.carts.set(res.data);
        }
      });
    }


    createOrder() {
      this.toast.showSwal("Create an Order?","Do you want to create an order?",()=>{
        this.http.get<ResultModel<string>>(`${api}/shoppingCarts/createOrder`, {
          headers: {
            "Authorization": "Bearer " + this.token()
          }
        }).subscribe((res) => {
          if (res.data) {
            this.getAll();
            this.toast.showToast("Successfull!",res.data,"info");
          }
        });
      },"Allow","Cancel")
      
    }
}
