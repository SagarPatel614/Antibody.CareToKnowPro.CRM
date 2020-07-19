import { Component, Input } from '@angular/core';


@Component({
  selector: "app-progress",
  templateUrl: './progress.component.html',
  styleUrls: ['./progress.component.css']
})
export class ProgressComponent {
  @Input() message = "Validating and extracting file";
  @Input() loading: boolean = false;
}
