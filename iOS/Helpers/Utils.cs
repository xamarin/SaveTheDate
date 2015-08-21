using System;
using Foundation;

namespace SaveTheDate.iOS.Helpers
{
    public static class Utils
    {
        public static NSObject Invoker;
        /// <summary>
        /// Ensures the invoked on main thread.
        /// </summary>
        /// <param name="action">Action to run on main thread.</param>
        public static void EnsureInvokedOnMainThread (Action action)
        {
            if (NSThread.Current.IsMainThread) {
                action ();
                return;
            }
            if (Invoker == null)
                Invoker = new NSObject ();

            Invoker.BeginInvokeOnMainThread(action);
        }

        public static NSDate ToNSDate(this DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            return (NSDate) date;
        }

    }
}

