import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { MyApplicationShortViewDTO } from '../../../models/MyApplicationShortViewDTO';

@Component({
  selector: 'app-short-view-my-applications',
  templateUrl: './short-view-my-applications.component.html',
  styleUrls: ['./short-view-my-applications.component.scss']
})
export class ShortViewMyApplicationsComponent{
  @Input() application: MyApplicationShortViewDTO;

  @Input() statusText: string;

  

  getStatusClass(): string[] {
    // Define your logic here to determine the CSS class based on statusText
    const classes: string[] = [];
    if (this.application.status === 'accepted') {
      classes.push('accepted');
    } else if (this.application.status === 'denied') {
      classes.push('denied');
    } else if (this.application.status === 'uploaded') {
      classes.push('uploaded');
    } else {
      classes.push('pending');
    }
    return classes;
  }

}

