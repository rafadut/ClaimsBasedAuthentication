using System;

namespace ClaimsBasedAuthentication.Controllers
{
    internal class Entry
    {
        public Entry()
        {
        }

        public string Message { get; internal set; }
        public DateTime EntryDate { get; internal set; }
    }
}