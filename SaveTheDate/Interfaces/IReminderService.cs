using System;

namespace SaveTheDate.Interfaces
{
    public interface IReminderService
    {
        void AddEvent(DateTime startTime, DateTime endTime, string title, string location, string message, Action<bool>callback, string id);
        bool EventExists(DateTime startTime, string title, string id);
        void RemoveEvent(DateTime startDate, string title, string id);
    }
}

