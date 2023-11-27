import { Component, ElementRef, ViewChild, AfterViewInit, Input } from '@angular/core';
import Chart from 'chart.js/auto';
import zoomPlugin from 'chartjs-plugin-zoom';
Chart.register(zoomPlugin);

@Component({
  selector: 'app-uno-chart',
  templateUrl: './uno-chart.component.html',
  styleUrls: ['./uno-chart.component.css']
})
export class UnoChartComponent implements AfterViewInit {
  @ViewChild('chartCanvas') chartCanvas!: ElementRef;
  @Input() xValues:number[] =[]
  @Input() yValues:number[] =[]

  ngAfterViewInit(): void {
    this.setCanvasSize();
    this.generateChart();
  }

  setCanvasSize() {
    const canvas: HTMLCanvasElement = this.chartCanvas.nativeElement;
    canvas.width = 800; 
    canvas.height = 400; 
  }

  generateChart() {
    const ctx = this.chartCanvas.nativeElement.getContext('2d');


    const data = this.xValues.map((xVal, index) => ({ x: xVal, y: this.yValues[index] }));

    new Chart(ctx, {
      type: 'line',
      data: {
        datasets: [{
          label: 'Sensor Data',
          data: data,
          borderWidth: 1
        }]
      },
      options: {
        scales: {
          x: {
            type: 'linear', 
            position: 'bottom',
            title: {
              display: true,
              text: 'x'
            }
          },
          y: {
            beginAtZero: true,
            title: {
              display: true,
              text: 'y'
            } 
          }
        },
        plugins: {
          zoom: {
            zoom: {
              wheel: {
                enabled: true,
              },
              pinch: {
                enabled: true
              },
            }
          }
        }
      }
    });
  }
}
