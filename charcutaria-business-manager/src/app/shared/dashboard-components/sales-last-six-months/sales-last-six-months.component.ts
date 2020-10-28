import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewEncapsulation } from '@angular/core';
import 'd3';
import * as c3 from 'c3';
import * as moment from 'moment';
import { SalesPerMonth } from '../../models/salesPerMonth';
import { OrderService } from '../../services/order/order.service';
import { NotificationService } from '../../services/notification/notification.service';

@Component({
  selector: 'sales-last-six-months',
  templateUrl: './sales-last-six-months.component.html',
  styleUrls: ['./sales-last-six-months.component.scss',
    '../../../../../node_modules/c3/c3.min.css'],
  encapsulation: ViewEncapsulation.None
})
export class SalesLastSixMonthsComponent implements OnInit {
  monthlySales: SalesPerMonth[] = [];

  constructor(private orderService: OrderService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.orderService.getSalesPerMonth().subscribe(r => {
      this.monthlySales = r;
      this.renderMonthlySales();
    },
      e => {
        this.notificationService.notifyHttpError(e);
      });
  }

  private renderMonthlySales() {
    setTimeout(() => {
      let profits: Array<any> = ['Lucro'];
      let sales: Array<any> = ['Vendas'];
      let months: Array<any> = [];
      this.monthlySales.forEach(v => {
        sales.push(v.totalSales);
        profits.push(v.totalProfit);
        let date = moment(v.date);
        months.push(`${date.format('MM')}/${date.format('YYYY')}`);
      });
      var chart = c3.generate({
        bindto: '#monthlysales', size: {
          height: 240
        },
        data: {
          type: 'spline',
          columns: [
            sales,
            profits
          ]
        },
        axis: {
          x: {
            type: 'category',
            categories: months
          }
        }
      });
    }, 1);
  }
  private getColorArray(qtd: number): Array<string> {
    let colors = [
      '#6666ff', //azul
      '#ff3333', //vermelho
      '#00b300', //verde
      '#ff9900', //Laranja
      '#660066', //Roxo
      '#ffcb80', //Amarelo
      '#ff869a', //Rosa
      '#2ed8b6', //Verde Claro
      '#4099ff', //Azul Claro
      '#666699' //Roxo fosco
    ];
    let result = [];
    for (let index = 0; index < qtd; index++) {
      if (index < colors.length)
        result.push(colors[index]);
      else {
        while (true) {
          let random = Math.floor(Math.random() * 16777215).toString(16);
          let existingColor = colors.filter(c => c == random);
          if (existingColor.length == 0) {
            result.push(random);
            break;
          }
        }
      }
    }
    return result;
  }

}
