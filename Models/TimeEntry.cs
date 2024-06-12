using System;
using System.Text.Json.Serialization;

namespace TimeApi.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        
        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }
        
        [JsonPropertyName("hostname")]
        public string HostName { get; set; }
    }
}