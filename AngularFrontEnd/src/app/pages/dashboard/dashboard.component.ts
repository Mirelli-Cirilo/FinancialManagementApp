import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { Dashboard } from '../../models/dashboard';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})

export class DashboardComponent implements OnInit {
  chart: any;

  constructor(private dashboardService: DashboardService) { 
    Chart.register(...registerables);
  }

  ngOnInit(): void {
    this.dashboardService.getTransactionDashboard().subscribe((data: Dashboard[]) => {
      this.createChart(data);
    });
  }

  createChart(data: Dashboard[]): void {
    const dates = data.map((item: Dashboard) => item.date);
    const descriptions = data.map((item: Dashboard) => item.description);
    const amounts = data.map((item: Dashboard) => item.totalAmount);

    this.chart = new Chart('canvas', {
      type: 'bar',
      data: {
        labels: dates,
        datasets: [
          {
            label: 'Total Amount',
            data: amounts,
            backgroundColor: 'rgba(54, 162, 235, 0.2)',
            borderColor: 'rgba(54, 162, 235, 1)',
            borderWidth: 1
          }
        ]
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
            type: 'linear'
          }
        },
        plugins: {
          tooltip: {
            callbacks: {
              label: function(context) {
                const index = context.dataIndex;
                const label = context.dataset.label || '';
                const amount = context.raw;
                const description = descriptions[index];
                return `${label}: ${amount}\nDescription: ${description}`;
              }
            }
          }
        }
      }
    });
  }
}