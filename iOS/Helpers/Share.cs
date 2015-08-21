using System;
using SaveTheDate.Interfaces;
using Foundation;
using UIKit;

namespace SaveTheDate.iOS.Helpers
{
    public class Share : IShare
    {
        public static UIViewController ViewController {get;set;}
        public async void ShareText (string text)
        {
            try
            {
                var items = new NSObject[] { new NSString (text) };
                var activityController = new UIActivityViewController (items, null);
                var vc = ViewController ?? UIApplication.SharedApplication.KeyWindow.RootViewController;
                await vc.PresentViewControllerAsync (activityController, true);
            }
            catch(Exception ex)
            {
                Xamarin.Insights.Report(ex);
            }
        }
           

        public string GetEmail ()
        {
            return string.Empty;
        }
    }
}

