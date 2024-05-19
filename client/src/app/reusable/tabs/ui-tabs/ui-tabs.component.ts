import { AfterContentInit, Component,  EventEmitter, Input, OnChanges, Output, SimpleChanges} from '@angular/core';

@Component({
  selector: 'app-ui-tabs',
  templateUrl: './ui-tabs.component.html',
  styleUrl: './ui-tabs.component.css'
})
export class UiTabsComponent implements OnChanges{
  @Input() itemPrice: number;
  @Input() tabs: string[];
  @Output() tabChanged = new EventEmitter<string>;
  activatedTab: string = '';

  activateTab(tab: string) {
    this.activatedTab = tab;
    this.tabChanged.emit(tab);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(this.tabs) {
      this.activatedTab = this.tabs[0];
    }
  }
}
