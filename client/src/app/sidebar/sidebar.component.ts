import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {

  constructor(){
    this.checkIfMobileSize();
  }
  
  selectedItem: string = 'Home'; 
  isSidebarOpen : boolean = true;
  isMobileSize: boolean = false;

  selectItem(item: string) {
    this.selectedItem = item;
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  checkIfMobileSize() {
    const mediaQuery = window.matchMedia('(max-width: 768px)');
    console.log(mediaQuery)
    this.isMobileSize = mediaQuery.matches;
  }
}
