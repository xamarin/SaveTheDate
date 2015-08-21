using System;
using System.Threading.Tasks;
using System.Net.Http;
using SaveTheDate.Helpers;
using DeviceInfo.Plugin;

namespace SaveTheDate
{
    public static class SaveTheDateHelper
    {

        public static void Share()
        {
            Xamarin.Insights.Track("Share");
            Utils.Sharer.ShareText(Strings.ShareText);
        }

        public static void AddToCalendar()
        {
            Xamarin.Insights.Track("AddToCalendar");

            //Start and End dates
            var start = new DateTime(2016, 4, 24, 12, 0, 0);
            var end = new DateTime(2016, 4, 28, 12, 0, 0);

            Utils.Reminder.AddEvent(start, end, "Your Event Name", "Event Location", string.Empty, (success) =>
                {
                    Settings.AddedToCalendar = success;
                    if (success)
                    {
                        Utils.Message.SendToast("Added to Calendar Successfully"); 
                    }
                }, "4242016400");
        }

        public static bool IsValidEmail(string email)
        {
            email = email.Trim();
            return email.IsValidEmail();
        }


        public static async Task<bool> RegisterForNotifications(string email)
        {

            var success = false;
            try
            {
                
                email = email.Trim();
                if (!email.IsValidEmail())
                {
                    Utils.Message.SendToast("Invalid Email Address.");
                    return false;
                }


                Settings.RegisteredEmail = email;

                Xamarin.Insights.Track("RegisteredForNotifications");
                Xamarin.Insights.Identify(Settings.RegisteredEmail,  Xamarin.Insights.Traits.Email, Settings.RegisteredEmail);

                //TODO: Implement your backend here
                using (var client = new HttpClient())
                {
                    /*var content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("email", email)
                        });
                    var result = await client.PostAsync("http://your-post-end-point", content);

                    success = result.IsSuccessStatusCode;*/

                    success = true;
                    Settings.IsRegistered = success;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Xamarin.Insights.Report(ex);
            }

            var title = "Registration Issue";
            var message = "Unable to process, please check your connection.";
            if (success)
            {
                message = "Thanks! You're on the list to be notified about EVENT.";
                title = "All Set!";
            }

            if (CrossDeviceInfo.Current.Platform == DeviceInfo.Plugin.Abstractions.Platform.iOS)
                Utils.Message.SendMessage(message, title);
            else
                Utils.Message.SendToast(message);

            return success;
        }


    }
}

