import { Component, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject, Observable } from 'rxjs';
import { WebcamImage, WebcamInitError } from 'ngx-webcam';
import { SnackBarService } from 'app/layout/services/snackbar.service';

@Component({
    selector: 'webcamdialog',
    templateUrl: './webcamdialog.component.html',
    styles: [`
    mat-form-field { width: 100%; }
    mat-card { height: auto; padding: 0px; }
    mat-card .card-label { color: black; margin-top: 8px; font-size: 16px; }
    .header { position: fixed; z-index: 100; padding: 10px; color: white; background-color: #039be5; }
    .card-header { background-color: #d6d6f6; color: black; margin-bottom: 20px; }
    mat-icon[mat-list-icon] { order: 10; }
    a, a label { cursor: pointer; }
  `],
    standalone: false
})
export class WebCamDialogComponent implements OnInit {

  dialogTitle: string = 'Take Photo';

  public webcamImage: WebcamImage = null;
  private trigger: Subject<void> = new Subject<void>();
  private nextWebcam: Subject<boolean | string> = new Subject<boolean | string>();

  //  Front camera video options
  videoOptions: MediaTrackConstraints = {
    width: { ideal: 400 },
    height: { ideal: 400 },
    facingMode: 'user'
  };

  image: any;
  imageBase64: any;
  imageFile: any;

  constructor(
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<WebCamDialogComponent>,
    private snackBarService: SnackBarService
  ) {}

  ngOnInit() {}

  public get triggerObservable(): Observable<void> {
    return this.trigger.asObservable();
  }

  public handleImage(webcamImage: WebcamImage): void {
    this.webcamImage = webcamImage;
    this.image = webcamImage.imageAsDataUrl;
    this.dialogRef.close({ data: this.image });
  }

  public triggerSnapshot(): void {
    this.trigger.next();
  }

  //  Handle camera permission or init errors
  public handleInitError(error: WebcamInitError): void {
    console.error('Webcam init error:', error);
    this.snackBarService.error('Unable to access camera. Please allow camera permission.');
  }

  public showNextWebcam(directionOrDeviceId: boolean | string): void {
    this.nextWebcam.next(directionOrDeviceId);
  }

  base64ToFile(imageBase64: string) {
    const date = new Date().valueOf();
    let text = '';
    const possibleText = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    for (let i = 0; i < 5; i++) {
      text += possibleText.charAt(Math.floor(Math.random() * possibleText.length));
    }
    const imageName = date + '.' + text + '.jpeg';
    const imageBlob = this.dataURItoBlob(imageBase64);
    return new File([imageBlob], imageName, { type: 'image/jpeg' });
  }

  dataURItoBlob(dataURI: string) {
    const byteString = window.atob(dataURI);
    const arrayBuffer = new ArrayBuffer(byteString.length);
    const int8Array = new Uint8Array(arrayBuffer);
    for (let i = 0; i < byteString.length; i++) {
      int8Array[i] = byteString.charCodeAt(i);
    }
    return new Blob([int8Array], { type: 'image/jpeg' });
  }
}
