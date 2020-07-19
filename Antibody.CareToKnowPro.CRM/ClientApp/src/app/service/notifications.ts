import { Component, OnInit } from '@angular/core';
declare var $: any;

export class NotificationService implements OnInit {

  constructor() { }

  error(msg) {
    this.showNotification(msg, "danger", 4000, "error");
  }

  warning(msg) {
    this.showNotification(msg, "warning", 4000, "warning");
  }

  success(msg) {
    this.showNotification(msg, "success", 4000, "check_circle_outline");
  }

  info(msg) {
    this.showNotification(msg, "info", 4000, "info");
  }

  show(msg) {
    this.showNotification(msg, "info", 4000, "notifications");
  }

  clear() {
    $('div.notification').remove();
    //$.notifyClose();
  }

  showNotification(msg, color, timer, icon){

    $.notify({
      icon: icon,
      message: msg

      },{
          type: color,
          timer: timer,
          placement: {
            from: "top",
            align: "right"
          },
          template: '<div data-notify="container" class="notification col-xl-4 col-lg-4 col-11 col-sm-4 col-md-4 alert alert-{0} alert-with-icon" role="alert">' +
            '<button mat-button  type="button" aria-hidden="true" class="close mat-button" data-notify="dismiss">  <i class="material-icons">close</i></button>' +
            '<i class="material-icons" data-notify="icon">' + icon + '</i> ' +
            '<span data-notify="title">{1}</span> ' +
            '<span data-notify="message">{2}</span>' +
            '<div class="progress" data-notify="progressbar">' +
              '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
            '</div>' +
            '<a href="{3}" target="{4}" data-notify="url"></a>' +
          '</div>'
      });
  }
  ngOnInit() {
  }

}
