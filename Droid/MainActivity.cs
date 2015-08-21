using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SaveTheDate.Droid.Helpers;
using Android.Support.Design.Widget;
using SaveTheDate.Helpers;
using Android.Support.V7.App;

namespace SaveTheDate.Droid
{
    [Activity(Label = "Save The Date", MainLauncher = true, Icon = "@drawable/ic_launcher", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Xamarin.Insights.Initialize(Xamarin.Insights.DebugModeKey, this);
            if(!string.IsNullOrWhiteSpace(Settings.RegisteredEmail))
                Xamarin.Insights.Identify(Settings.RegisteredEmail,  Xamarin.Insights.Traits.Email, Settings.RegisteredEmail);
            

            Utils.Sharer = new Share();
            Utils.Reminder = new ReminderService();
            Utils.Message = new MessageDialog();

            AndroidUtils.SnackbarView = FindViewById<LinearLayout>(Resource.Id.main);
            AndroidUtils.Context = this;

            var email = FindViewById<EditText>(Resource.Id.text_email);
            var notify = FindViewById<Button>(Resource.Id.button_notify);
            var share = FindViewById<Button>(Resource.Id.button_share);
            var calendar = FindViewById<Button>(Resource.Id.button_calendar);
            var progress = FindViewById<ProgressBar>(Resource.Id.progressBar);

            email.Text = Utils.Sharer.GetEmail();
            email.TextChanged += (sender, e) => 
                {
                    notify.Enabled = SaveTheDateHelper.IsValidEmail(email.Text);
                };

            notify.Enabled = SaveTheDateHelper.IsValidEmail(email.Text);
           
            share.Click += (sender, e) => 
                SaveTheDateHelper.Share();

            calendar.Click += (sender, e) => 
                SaveTheDateHelper.AddToCalendar();

            notify.Click += async (sender, e) => 
                {
                    if (Settings.IsRegistered)
                    {
                        Settings.IsRegistered = false;
                        SetupUI();
                        return;
                    }
                    email.Enabled = notify.Enabled = calendar.Enabled = share.Enabled = false;
                    progress.Visibility = ViewStates.Visible;
                    await SaveTheDateHelper.RegisterForNotifications(email.Text);
                    progress.Visibility = ViewStates.Invisible;
                    email.Enabled = notify.Enabled = calendar.Enabled = share.Enabled = true;
                    SetupUI();
                };

            SetupUI();
        }

        private void SetupUI()
        {
            var emailLayout = FindViewById<TextInputLayout>(Resource.Id.text_email_layout);
            var notify = FindViewById<Button>(Resource.Id.button_notify);
            var emailTextView = FindViewById<TextView>(Resource.Id.email_text);

                
            if (Settings.IsRegistered)
            {
                emailLayout.Visibility = ViewStates.Invisible;
                notify.Text = "          Register Again          ";
                emailTextView.Text = "Subscribed as: " + Settings.RegisteredEmail;
            }
            else
            {

                emailLayout.Visibility = ViewStates.Visible;
                notify.Text = "          Notify Me          ";
                emailTextView.Text = "Enter your email address to be the first to know when tickets are on sale.";
            }

        }

        protected override void OnStart()
        {
            base.OnStart();
            AndroidUtils.Context = this;
        }
    }
}


