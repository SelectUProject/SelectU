import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
class NavbarComponent {
  @Input() title: string = 'Title';
}

export default NavbarComponent;
