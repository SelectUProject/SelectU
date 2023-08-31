import { Component, Input } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
class NavbarComponent {
  @Input() title: string = 'Title';
  admissionName = environment.admissionName;
}

export default NavbarComponent;
