using System;
using System.Text.Json.Serialization;
namespace GarbageDBTest.Domain.Dtos.Request
{
    public class EventCreateRequest
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan Time { get; set; }
        public string Colony { get; set; }
        public int RequiredPersons { get; set; }
        public string Features { get; set; }
        public string Sponsor { get; set; }
        public string Specialconditions { get; set; }
        public string Reason { get; set; }
    }
}