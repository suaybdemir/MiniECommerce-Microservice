import { Component, computed, signal } from '@angular/core';
import { FlexiGridModule } from 'flexi-grid';
import { TrCurrencyPipe } from 'tr-currency';
import { OrderModel } from '../../models/order.model';
import { api } from '../../constants';
import { ResultModel } from '../../models/result.model';
import { HttpClient } from '@angular/common/http';
import { FlexiToastService } from 'flexi-toast';

@Component({
  selector: 'app-orders',
  imports: [FlexiGridModule,TrCurrencyPipe],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {
  orders = signal<OrderModel[]>([]);
  token = signal<string>("");

  total = computed(()=>{
      let t = 0;
      this.orders().forEach((val)=>{
        t += val.price * val.quantity
        val.price = val.price*val.quantity;
      });
  
      return t;
    });

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
      this.http.get<ResultModel<OrderModel[]>>(`${api}/orders/getall`, {
        headers: {
          "Authorization": "Bearer " + this.token()
        }
      }).subscribe(res => {
          this.orders.set(res.data!);
        
      });
    }
}
