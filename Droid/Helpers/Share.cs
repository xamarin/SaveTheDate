using System;
using Android.Accounts;
using Android.Util;
using Android.Content;
using SaveTheDate.Interfaces;
using SaveTheDate.Helpers;

namespace SaveTheDate.Droid.Helpers
{
    public class Share : IShare
    {
        public void ShareText (string text)
        {
            var intent = new Intent (Intent.ActionSend);
            intent.SetType ("text/plain");
            intent.PutExtra (Intent.ExtraText, text);
            AndroidUtils.Context.StartActivity (Intent.CreateChooser (intent, "Share Event"));
        }



        public string GetEmail ()
        {
            var emailPattern = Patterns.EmailAddress; // API level 8+
            var accounts = AccountManager.Get (AndroidUtils.Context).GetAccounts ();
            foreach (var account in accounts) {
                if (emailPattern.Matcher (account.Name).Matches ()) {
                    return account.Name;
                }
            }

            return string.Empty;
        }
    }
}

