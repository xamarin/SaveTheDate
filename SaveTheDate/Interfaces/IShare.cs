using System;

namespace SaveTheDate.Interfaces
{
    public interface IShare
    {
        void ShareText(string text);
        string GetEmail();
    }
}

