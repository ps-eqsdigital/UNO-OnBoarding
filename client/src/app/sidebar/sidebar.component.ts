import { Component } from '@angular/core';

// Enum defining selectable items
enum SelectableItems {
  Home = 'Home',
  Dashboards = 'Dashboards',
  Sensors = 'Sensors',
  Users = 'Users',
  Settings = 'Settings',
  Logout = 'Logout'
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {

  constructor(){
    this.checkIfMobileSize();
  }
  
  selectedItem: string = SelectableItems.Home; 
  isSidebarOpen: boolean = true;
  isMobileSize: boolean = false;
  selectableItems = SelectableItems;

  selectItem(item: string) {
    this.selectedItem = item;
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  checkIfMobileSize() {
    const mediaQuery = window.matchMedia('(max-width: 768px)');
    this.isMobileSize = mediaQuery.matches;
  }
}
