import { Component, TemplateRef } from '@angular/core';
import { ToastService } from 'src/app/providers/toast.service';

@Component({
  selector: 'app-toasts-container',
  templateUrl: './toasts-container.component.html',
  styleUrls: ['./toasts-container.component.scss'],
  host: {'class': 'toast-container position-fixed top-10 end-0 p-3', 'style': 'z-index: 1200'}
})
export class ToastsContainerComponent {
  constructor(public toastService: ToastService) {}

  isTemplate(toast: any) {
		return toast.textOrTpl instanceof TemplateRef;
	}
}
