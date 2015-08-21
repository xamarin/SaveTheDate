using System;
using UIKit;
using SaveTheDate.Interfaces;
using GCDiscreetNotification;

namespace SaveTheDate.iOS.Helpers
{
    public class MessageDialog : IMessage
    {

        public void SendMessage(string message, string title = null)
        {
            Utils.EnsureInvokedOnMainThread(() =>
                {
                    var alertView = new UIAlertView(title ?? string.Empty, message, null, "OK");
                    alertView.Show();
                });
        }


        public void SendToast(string message)
        {
            Utils.EnsureInvokedOnMainThread(() =>
                {
                    var notificationView = new GCDiscreetNotificationView(
                                               text: message,
                                               activity: false,
                                               presentationMode: GCDNPresentationMode.Bottom,
                                               view: UIApplication.SharedApplication.KeyWindow
                                           );

                    notificationView.ShowAndDismissAfter(4);
                });
        }

        public void SendConfirmation(string message, string title, System.Action<bool> confirmationAction)
        {
            Utils.EnsureInvokedOnMainThread(() =>
                {
                    var alertView = new UIAlertView(title ?? string.Empty, message, null, "OK", "Cancel");
                    alertView.Clicked += (sender, e) =>
                    {
                        confirmationAction(e.ButtonIndex == 0);
                    };
                    alertView.Show();
                });
        }
    }
}

