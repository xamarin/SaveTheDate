using System;
using SaveTheDate.Interfaces;

namespace SaveTheDate.Helpers
{
    public static partial class Utils
    {
        public static IShare  Sharer { get; set; }
        public static IReminderService  Reminder { get; set; }
        public static IMessage Message { get; set; }
    }
}

