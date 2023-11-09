import { Component, Input } from '@angular/core';


@Component({
  selector: 'app-slide-toggle',
  templateUrl: './slide-toggle.component.html',
  styleUrls: ['./slide-toggle.component.css']
})
export class SlideToggleComponent {
  @Input() public text:string=''
}
