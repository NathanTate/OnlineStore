import { AfterViewInit, Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, ViewChild } from "@angular/core";
import { Member, Review } from "../../_models/Review";
import { faEllipsisVertical, faTrash } from "@fortawesome/free-solid-svg-icons";
import { faEdit } from "@fortawesome/free-regular-svg-icons";

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrl: './review.component.css'
})
export class ReviewComponent implements OnInit{
  private detailsEl: HTMLDetailsElement;
  @ViewChild('details') 
    set dRef(ref: ElementRef) {
      this.detailsEl = ref?.nativeElement;
    };
  @Input({required: true}) review: Review;
  @Input() canDelete: boolean = false;
  @Input() currentUserId: string;
  iconMore = faEllipsisVertical;
  iconTrash = faTrash;
  iconEdit = faEdit;
  member: Member;

  ngOnInit(): void {
    this.member = this.review.member;
    this.canDelete = this.currentUserId === this.member.id;
  }

  @HostListener('document:click', ['$event']) onClick(event: MouseEvent) {
    if(!this.detailsEl) return;
    if(!this.detailsEl.contains(event.target as Node)) {
      this.detailsEl.removeAttribute('open');
    }
  }

  @Output() deleteReview = new EventEmitter<number>();
}