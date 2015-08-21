using System;
        
using UIKit;
using Social;
using Foundation;
using SaveTheDate.iOS.CustomControls;
using SaveTheDate.iOS.Helpers;
using CoreGraphics;
using SaveTheDate.Helpers;

namespace SaveTheDate.iOS
{
    public partial class ViewController : BaseViewController
    {
        public ViewController(IntPtr handle)
            : base(handle)
        {        
        }

        UIView tempBackgroundView;
        CGRect defaultLogoFrame;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SaveTheDate.Helpers.Utils.Sharer = new Share();
            SaveTheDate.Helpers.Utils.Message = new MessageDialog();

            tempBackgroundView = new UIView(View.Frame);
            tempBackgroundView.BackgroundColor = View.BackgroundColor;
            View.AddSubview(tempBackgroundView);

            defaultLogoFrame = imgLogo.Frame;
            imgLogo.Frame = new CGRect(View.Center, new CGSize(118, 100));
            imgLogo.Center = View.Center;
            View.BringSubviewToFront(imgLogo);

            tbxEmailAddress.AccessibilityIdentifier = "text_email";
            btnNotifyMe.AccessibilityIdentifier = "button_notify";
            btnShare.AccessibilityIdentifier = "button_share";
            btnAddToCalendar.AccessibilityIdentifier = "button_calendar";

            SetupUI();
            SetupUI2();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            UIView.Animate(0.6, 0.2, UIViewAnimationOptions.CurveLinear,
                () =>
                {
                    if(imgLogo != null)
                        imgLogo.Frame = defaultLogoFrame;
                }, () =>
                {
                });

            UIView.Animate(0.3, 0.8, UIViewAnimationOptions.CurveEaseOut,
                () =>
                {
                    if(tempBackgroundView != null)
                        tempBackgroundView.Alpha = 0;
                }, () =>
                {
                    tempBackgroundView = null;
                });
        }

        public override void DidReceiveMemoryWarning()
        {        
            base.DidReceiveMemoryWarning();        
            // Release any cached data, images, etc that aren't in use.        
        }

        public override void OnKeyboardChanged(bool visible, CoreGraphics.CGRect keyboardFrame)
        {
            base.OnKeyboardChanged(visible, keyboardFrame);
            if (visible == true)
            {
                emailView.Frame = new CoreGraphics.CGRect(emailView.Frame.X, emailView.Frame.Y - 60, emailView.Frame.Width, emailView.Frame.Height);

                var transform = CGAffineTransform.MakeIdentity();
                transform.Scale(0.6f, 0.6f);     

                UIView.Animate(0.6, 0, UIViewAnimationOptions.CurveEaseIn,
                    () =>
                    {
                        if(imgLogo == null || lblDate == null)
                            return;
                        
                        imgLogo.Transform = transform;
                        imgLogo.Frame = new CGRect(imgLogo.Frame.X, imgLogo.Frame.Y - 20, imgLogo.Frame.Width, imgLogo.Frame.Height);

                        lblDate.Transform = transform;
                        lblDate.Frame = new CGRect(lblDate.Frame.X, lblDate.Frame.Y - 40, lblDate.Frame.Width, lblDate.Frame.Height);
                    }, () =>
                    {
                    });
            }
            else
            {
                emailView.Frame = new CoreGraphics.CGRect(emailView.Frame.X, emailView.Frame.Y + 60, emailView.Frame.Width, emailView.Frame.Height);
                var transform = CGAffineTransform.MakeIdentity();
                transform.Scale(1.0f, 1.0f);     

                UIView.Animate(0.6, 0, UIViewAnimationOptions.CurveEaseIn,
                    () =>
                    {
                        if(imgLogo == null || lblDate == null)
                            return;
                        
                        imgLogo.Transform = transform;
                        imgLogo.Frame = new CGRect(imgLogo.Frame.X, imgLogo.Frame.Y + 20, imgLogo.Frame.Width, imgLogo.Frame.Height);

                        lblDate.Transform = transform;
                        lblDate.Frame = new CGRect(lblDate.Frame.X, lblDate.Frame.Y + 40, lblDate.Frame.Width, lblDate.Frame.Height);

                    }, () =>
                    {
                    });
            }
        }

        void SetupUI2()
        {
            if (Settings.IsRegistered)
            {
                line.Hidden = true;
                tbxEmailAddress.Hidden = true;
                btnNotifyMe.SetTitle("Register Again", UIControlState.Normal);
                lblEmail.Text = "Subscribed as: " + Settings.RegisteredEmail;
            }
            else
            {
                line.Hidden = false;
                tbxEmailAddress.Hidden = false;
                btnNotifyMe.SetTitle("Notify Me", UIControlState.Normal);
                lblEmail.Text = "Enter your email address to be the first to know when tickets are on sale.";
            }

            if (Settings.IsRegistered)
                btnNotifyMe.Enabled = true;
            else
                btnNotifyMe.Enabled = SaveTheDateHelper.IsValidEmail(tbxEmailAddress.Text);
            
        }

        void SetupUI()
        {
            //Tap to hide keyboard
            DismissKeyboardOnBackgroundTap();
            RegisterForKeyboardNotifications();

            btnNotifyMe.Enabled = false;

            //Textbox 
            tbxEmailAddress.AutocorrectionType = UITextAutocorrectionType.No;
            tbxEmailAddress.AttributedPlaceholder = new NSAttributedString("Email Address", new UIStringAttributes{ ForegroundColor = UIColor.FromRGB(73, 96, 134) });
            tbxEmailAddress.EditingChanged += EmailAddressTextChanged;
            tbxEmailAddress.ReturnKeyType = UIReturnKeyType.Done;
            tbxEmailAddress.ShouldReturn += ((textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            });
        }

        void EmailAddressTextChanged(object sender, EventArgs e)
        {
            if (Settings.IsRegistered)
                btnNotifyMe.Enabled = true;
            else
                btnNotifyMe.Enabled = SaveTheDateHelper.IsValidEmail(tbxEmailAddress.Text);
        }

        #region Buttons

        partial void btnShare_TouchUpInside(UIButton sender)
        {
            Share.ViewController = this;
            SaveTheDateHelper.Share();
        }

        async partial void btnNotifyMe_TouchUpInside(NotifyButton sender)
        {
            ResignFirstResponder();

            if(Settings.IsRegistered)
            {
                Settings.IsRegistered = false;
                SetupUI2();
                return;
            }
            progressBar.StartAnimating();
            btnShare.Enabled = false;
            btnNotifyMe.Enabled = false;
            btnAddToCalendar.Enabled = false;
            await SaveTheDateHelper.RegisterForNotifications(tbxEmailAddress.Text);
            btnShare.Enabled = true;
            btnNotifyMe.Enabled = true;
            btnAddToCalendar.Enabled = true;
            progressBar.StopAnimating();
            SetupUI2();
        }

        partial void btnAddToCalendar_TouchUpInside(UIButton sender)
        {

            SaveTheDate.Helpers.Utils.Reminder = new ReminderService();
            SaveTheDateHelper.AddToCalendar();
        }

        #endregion
    }
}
