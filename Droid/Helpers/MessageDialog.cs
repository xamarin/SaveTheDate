using System;
using Android.App;
using Android.Widget;
using SaveTheDate.Interfaces;
using Android.Support.Design.Widget;

namespace SaveTheDate.Droid.Helpers
{
    public class MessageDialog : IMessage
    {

        public static void SendMessage(Activity activity, string message, string title = null)
        {
            var builder = new AlertDialog.Builder(activity);
            builder
                .SetTitle(title ?? string.Empty)
                .SetMessage(message)
                .SetPositiveButton(Android.Resource.String.Ok, delegate
                    {

                    });

            AlertDialog alert = builder.Create();
            alert.Show();
        }

        public void SendMessage(string message, string title = null)
        {
            var activity = AndroidUtils.Context as Activity;
            var builder = new AlertDialog.Builder(activity);
            builder
                .SetTitle(title ?? string.Empty)
                .SetMessage(message)
                .SetPositiveButton(Android.Resource.String.Ok, delegate
                    {

                    });             

            AlertDialog alert = builder.Create();
            alert.Show();
        }


        public void SendToast(string message)
        {
            Snackbar.Make (AndroidUtils.SnackbarView, message, Snackbar.LengthLong)
                .SetAction ("OK", (v) => { })
                .Show ();

        }


        public void SendConfirmation(string message, string title, System.Action<bool> confirmationAction)
        {
            var activity = AndroidUtils.Context as Activity;
            var builder = new Android.Support.V7.App.AlertDialog.Builder(activity);
            builder
                .SetTitle(title ?? string.Empty)
                .SetMessage(message)
                .SetPositiveButton(Android.Resource.String.Ok, delegate
                    {
                        confirmationAction(true);
                    }).SetNegativeButton(Android.Resource.String.Cancel, delegate
                        {
                            confirmationAction(false);
                        });

            var alert = builder.Create();
            alert.Show();
        }
    }
}

