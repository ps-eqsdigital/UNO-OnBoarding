import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  selectedItem: string = 'Home'; // Initially select "Home"

  selectItem(item: string) {
    this.selectedItem = item;
  }
}
