// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SaveTheDate.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton Button { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnAddToCalendar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		SaveTheDate.iOS.CustomControls.NotifyButton btnNotifyMe { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnShare { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView emailView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView imgLogo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblDate { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblEmail { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblSaveTheDate { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView line { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIActivityIndicatorView progressBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField tbxEmailAddress { get; set; }

		[Action ("btnAddToCalendar_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btnAddToCalendar_TouchUpInside (UIButton sender);

		[Action ("btnNotifyMe_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btnNotifyMe_TouchUpInside (SaveTheDate.iOS.CustomControls.NotifyButton sender);

		[Action ("btnShare_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btnShare_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (btnAddToCalendar != null) {
				btnAddToCalendar.Dispose ();
				btnAddToCalendar = null;
			}
			if (btnNotifyMe != null) {
				btnNotifyMe.Dispose ();
				btnNotifyMe = null;
			}
			if (btnShare != null) {
				btnShare.Dispose ();
				btnShare = null;
			}
			if (emailView != null) {
				emailView.Dispose ();
				emailView = null;
			}
			if (imgLogo != null) {
				imgLogo.Dispose ();
				imgLogo = null;
			}
			if (lblDate != null) {
				lblDate.Dispose ();
				lblDate = null;
			}
			if (lblEmail != null) {
				lblEmail.Dispose ();
				lblEmail = null;
			}
			if (lblSaveTheDate != null) {
				lblSaveTheDate.Dispose ();
				lblSaveTheDate = null;
			}
			if (line != null) {
				line.Dispose ();
				line = null;
			}
			if (progressBar != null) {
				progressBar.Dispose ();
				progressBar = null;
			}
			if (tbxEmailAddress != null) {
				tbxEmailAddress.Dispose ();
				tbxEmailAddress = null;
			}
		}
	}
}
