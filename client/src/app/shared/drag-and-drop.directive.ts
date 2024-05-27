import { Directive, EventEmitter, HostBinding, HostListener, Output } from '@angular/core';

@Directive({
  selector: '[appDragAndDrop]'
})
export class DragAndDropDirective {
  @Output() onFileDropped = new EventEmitter<FileList>()
  @HostBinding('style.opacity') private workspace__opacity = '1';

  constructor() { }

  @HostListener('dragover', ['$event']) onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.workspace__opacity = '0.5';
  }

  @HostListener('dragleave', ['$event']) onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.workspace__opacity = '1';
  }

  @HostListener('drop', ['$event']) onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.workspace__opacity = '1';
    let files = event.dataTransfer!.files;
    if(files.length > 0) {
      this.onFileDropped.emit(files);
    }
  }

}
