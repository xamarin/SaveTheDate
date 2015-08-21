using System;
using Android.Provider;
using Android.Content;
using SaveTheDate.Interfaces;
using Android.App;
using Java.Util;

namespace SaveTheDate.Droid.Helpers
{
    public class ReminderService : IReminderService
    {

        #region IReminderService implementation

        public void AddEvent (DateTime startTime, DateTime endTime, string title, string location, string message, Action<bool> callback, string id)
        {
            var calId = GetCalendarId ();

            if (calId == -1) {
                callback (false);
                return;
            }

            ContentValues eventValues = new ContentValues ();
            eventValues.Put (CalendarContract.Events.InterfaceConsts.CalendarId, calId);
            eventValues.Put (CalendarContract.Events.InterfaceConsts.Title, title);
            eventValues.Put (CalendarContract.Events.InterfaceConsts.EventLocation, location);
            eventValues.Put (CalendarContract.Events.InterfaceConsts.AllDay, true);
            eventValues.Put (CalendarContract.Events.InterfaceConsts.Description, location + "\n\n" + message);
            eventValues.Put (CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS (startTime));
            eventValues.Put (CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMS (endTime));

            // GitHub issue #9 : Event start and end times need timezone support.
            // https://github.com/xamarin/monodroid-samples/issues/9
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, "US/Eastern");
            eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, "US/Eastern");

            try{
                var uri = AndroidUtils.Context.ContentResolver.Insert (CalendarContract.Events.ContentUri, eventValues);
                Console.WriteLine ("Uri for new event: {0}", uri);
                long eventID = 0;

                if (!long.TryParse(uri.LastPathSegment, out eventID)) {
                    callback (false);
                    return;
                }


                SaveTheDate.Helpers.Settings.AddEventId (id, eventID);

                var reminderValues = new ContentValues ();
                reminderValues.Put (CalendarContract.Reminders.InterfaceConsts.Minutes, 60*24);
                reminderValues.Put (CalendarContract.Reminders.InterfaceConsts.EventId, eventID);
                reminderValues.Put (CalendarContract.Reminders.InterfaceConsts.Method, (int)RemindersMethod.Alert);
                uri = AndroidUtils.Context.ContentResolver.Insert(CalendarContract.Reminders.ContentUri, reminderValues);
            }
            catch(Exception Exception){
                callback (false);
                return;
            }
            callback (true);
        }

        static String[]  INSTANCE_PROJECTION = {
            CalendarContract.Instances.EventId,// 0
            CalendarContract.Instances.Begin,// 1
            CalendarContract.EventsColumns.Title// 2
        };

        // The indices for the projection array above.
        const int PROJECTION_ID_INDEX = 0;
        public bool EventExists (DateTime startTime, string title, string id)
        {

            try{
                // Specify the date range you want to search for recurring
                // event instances
                var startMillis = GetDateTimeMS(startTime.AddDays(-3));
                var endMillis = GetDateTimeMS(startTime.AddDays(3));

                var cr = AndroidUtils.Context.ContentResolver;

                // The Title of the recurring event whose instances you are searching
                // for in the Instances table
                var selection = CalendarContract.EventsColumns.Title + " = ?";
                var selectionArgs = new [] {title};

                // Construct the query with the desired date range.
                var builder = CalendarContract.Instances.ContentUri.BuildUpon();
                ContentUris.AppendId(builder, startMillis);
                ContentUris.AppendId(builder, endMillis);

                // Submit the query
                var cur =  cr.Query(builder.Build(), 
                    INSTANCE_PROJECTION, 
                    selection, 
                    selectionArgs, 
                    null);

                //there should only be one of these in here :)
                while (cur.MoveToNext()) {
                    var eventID = cur.GetLong(PROJECTION_ID_INDEX);
                    SaveTheDate.Helpers.Settings.AddEventId (id, eventID);
                    return true;
                }


                return false;//event doesn't exists!
            }
            catch(Exception ex) {
                //LOG THIS
            }

            return SaveTheDate.Helpers.Settings.GetEventId(id) != -1;
        }

        public void RemoveEvent (DateTime startDate, string title, string id)
        {
            var eventID = SaveTheDate.Helpers.Settings.GetEventId (id);
            var cr = AndroidUtils.Context.ContentResolver;

            var deleteUri = ContentUris.WithAppendedId(CalendarContract.Events.ContentUri, eventID);
            var rows = cr.Delete(deleteUri, null, null);
            SaveTheDate.Helpers.Settings.AddEventId (id, -1);
        }

        int GetCalendarId()
        {

            var calendarsUri = CalendarContract.Calendars.ContentUri;

            string[] calendarsProjection = {
                CalendarContract.Calendars.InterfaceConsts.Id,
                CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
                CalendarContract.Calendars.InterfaceConsts.AccountName
            };


           
            if (AndroidUtils.Context == null)
                return -1;

            var cursor = AndroidUtils.Context.ManagedQuery(calendarsUri, calendarsProjection, null, null, null);

            if (cursor.Count == 0)
                return -1;


            cursor.MoveToPosition(0);
            int calId = cursor.GetInt (cursor.GetColumnIndex (calendarsProjection [0]));
            return calId;
        }

        long GetDateTimeMS (DateTime date)
        {
            var c = Calendar.GetInstance (Java.Util.TimeZone.GetTimeZone("US/Eastern"));

            c.Set (CalendarField.DayOfMonth, date.Day);
            c.Set (CalendarField.HourOfDay, date.Hour);
            c.Set (CalendarField.Minute, date.Minute);
            c.Set (CalendarField.Month, date.Month - 1);
            c.Set (CalendarField.Year, date.Year);

            return c.TimeInMillis;
        }

        #endregion
    }
}

