import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-options-list',
  templateUrl: './options-list.component.html',
  styleUrls: ['./options-list.component.css'],

})
export class OptionsListComponent {
  @Input() public data: any[] = [];
  @Input() public placeholder:string = '';

}
