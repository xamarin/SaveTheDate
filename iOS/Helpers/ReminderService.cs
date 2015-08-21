using System;
using UIKit;
using EventKit;
using Foundation;
using System.Linq;
using SaveTheDate.Interfaces;

namespace SaveTheDate.iOS.Helpers
{
    public class ReminderService : IReminderService
    {
        public static EKEventStore eventStore;

        public static EKEventStore EventStore { get { return eventStore ?? (eventStore = new EKEventStore ()); } }

        public void AddEvent (DateTime startDate, DateTime endDate, string title, string location, string description, Action<bool> callback, string id)
        {
            EventStore.RequestAccess (EKEntityType.Event, 
                (granted, e) =>
                {
                    if (granted)
                    {
                        eventStore = null;
                        AddEventForReal(startDate, endDate, title, location, description, callback, id);
                    }
                    else
                    {
                        eventStore = null;
                        callback(false);
                        Utils.EnsureInvokedOnMainThread(() => 
                        new UIAlertView("Calendar Access Denied", 
                                "Oops! Our worker monkeys were unable to add Xamarin Evole. Looks like they need access to your calendar.", null,
                                "OK", null).Show()
                        );
                    }
                });
        }

        private EKEvent FindEvent(DateTime startDate, string title)
        {
            var store = EventStore;
            var calendar = store.DefaultCalendarForNewEvents;
            if (calendar == null)
                return null;


            NSPredicate predicate = store.PredicateForEvents (startDate.Date.AddDays(-3).ToNSDate(), startDate.Date.AddDays(3).ToNSDate(), new EKCalendar[]{ calendar });

            var events = store.EventsMatching (predicate);
            if (events == null)
                return null;

            return events.FirstOrDefault (e => e.Title.Equals (title, StringComparison.InvariantCultureIgnoreCase));

        }


        public bool EventExists(DateTime startDate, string title, string id)
        {

            return FindEvent (startDate, title) != null;
        }

        public void RemoveEvent(DateTime startDate, string title, string id)
        {

            var store = EventStore;
            var calendar = store.DefaultCalendarForNewEvents;
            if (calendar == null)
                return;

            var foundEvent = FindEvent (startDate, title);

            if (foundEvent != null) {
                NSError error; 
                store.RemoveEvent (foundEvent, EKSpan.ThisEvent, true, out error);
            }
        }

        private void AddEventForReal (DateTime startDate, DateTime endDate, string title, string location, string description, Action<bool> callback, string id)
        {
            var store = EventStore;
            var calendar = store.DefaultCalendarForNewEvents;

            if (calendar == null) {
                callback (false);
                Utils.EnsureInvokedOnMainThread(() => 
                    new UIAlertView ("No Calendars", "This is rather embarrassing for us to tell you, but you don't seem to have a calendar. Please configure a calendar to add session.", null, "OK", null).Show ()
                );
                return;
            }



            if (EventExists(startDate, title, id)) {
                callback (true);
                return;
            }

            var newEvent = EKEvent.FromStore (store);
            newEvent.Title = title;
            newEvent.Notes = description;
            newEvent.Calendar = calendar;
            newEvent.TimeZone = NSTimeZone.FromName("US/Eastern");
            newEvent.StartDate = startDate.ToNSDate();
            newEvent.EndDate = endDate.ToNSDate();
            newEvent.AllDay = true;
            newEvent.Location = location;
            newEvent.Availability = EKEventAvailability.Busy;
            NSError error; 
            store.SaveEvent (newEvent, EKSpan.ThisEvent, true, out error);

            if (error == null)
                callback (true);
            else {
                callback (false);
                Utils.EnsureInvokedOnMainThread(() => 
                    new UIAlertView ("Sorry about the mess.", "Something went berserk and we couldn't add the session to your calendar, but it should be fixed now. Try again, please!", null, "OK", null).Show ()
                );
            }
        }


    }
}

