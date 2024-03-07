import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-room-invite',
  templateUrl: './room-invite.component.html',
  styleUrls: ['./room-invite.component.scss']
})
export class RoomInviteComponent {
  public linkForm: FormGroup;
  private link: string;

  constructor(public dialogRef: MatDialogRef<RoomInviteComponent>) {
    this.link = window.location.href;
    this.linkForm = new FormGroup({
      link: new FormControl(this.link, Validators.required)
    });
  }

  public onCopyLink(): void {
    navigator.clipboard.writeText(this.link);
  }

  public onClose(): void {
    this.dialogRef.close();
  }
}
