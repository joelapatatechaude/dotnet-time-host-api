using System;

namespace TimeApi.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string HostName { get; set; }
    }
}
