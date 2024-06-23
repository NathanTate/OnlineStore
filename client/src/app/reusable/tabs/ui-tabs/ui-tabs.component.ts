import { AfterContentInit, Component,  EventEmitter, Input, OnChanges, Output, SimpleChanges} from '@angular/core';

@Component({
  selector: 'app-ui-tabs',
  templateUrl: './ui-tabs.component.html',
  styleUrl: './ui-tabs.component.css'
})
export class UiTabsComponent implements OnChanges{
  @Input() tabs: string[];
  @Input() direction: string = 'horizontal';
  @Input() gap: string = '0.25rem';
  @Output() tabChanged = new EventEmitter<string>;
  @Input() defaultTab: string = ''
  activatedTab: string = '';

  activateTab(tab: string) {
    this.activatedTab = tab;
    this.tabChanged.emit(tab);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(this.tabs && !this.defaultTab) {
      this.activatedTab = this.tabs[0];
    } else {
      this.activatedTab = this.defaultTab
    }
  }
}
