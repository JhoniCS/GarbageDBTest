using System;
using System.Text.Json.Serialization;

namespace GarbageDBTest.Domain.Dtos.Response
{
    public class EventResponse
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Colony { get; set; }
        public int? Geoubication { get; set; }
        public int? RequiredPersons { get; set; }
        public string Features { get; set; }
        public string Sponsor { get; set; }
        public string Specialconditions { get; set; }
    }
}