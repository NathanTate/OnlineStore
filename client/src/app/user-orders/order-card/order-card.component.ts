import { Component, EventEmitter, Input, Output } from '@angular/core';
import { OrderHeader } from '../../_models/Order';
import { faJediOrder } from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-order-card',
  templateUrl: './order-card.component.html',
  styleUrl: './order-card.component.css'
})
export class OrderCardComponent {
  @Input() order: OrderHeader;
  @Output() onCancel = new EventEmitter<number>();
  iconOrder = faJediOrder
}
