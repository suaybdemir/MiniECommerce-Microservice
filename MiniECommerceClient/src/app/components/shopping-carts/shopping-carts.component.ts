import { Component, computed } from '@angular/core';
import { CartsService } from '../../services/carts.service';
import { FlexiGridModule }from 'flexi-grid'
import { ShoppingCartModel } from '../../models/shopping-cart.model';
import { TrCurrencyPipe } from 'tr-currency';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-shopping-carts',
  standalone: true,
  imports: [FlexiGridModule,TrCurrencyPipe,CommonModule],
  templateUrl: './shopping-carts.component.html',
  styleUrl: './shopping-carts.component.css'
})
export class ShoppingCartsComponent{

  total = computed(()=>{
    let t = 0;
    this.cart.carts().forEach((val)=>{
      t += val.productPrice * val.quantity
      val.totalPrice = val.productPrice*val.quantity;
    });

    return t;
  });

  constructor(
    public cart: CartsService
  ){}

  
}




