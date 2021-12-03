using System;
using System.Collections.Generic;
using Macross.Json.Extensions;
using System.Text.Json.Serialization;




#nullable disable


namespace GarbageDBTest.Domain.Entities
{
    public partial class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } //usuario
        public DateTime? Date { get; set; } //usuario
        public TimeSpan? Time { get; set; } //usuario
        public string Colony { get; set; } //usuario
        public int? Geoubication { get; set; } 
        public int? RequiredPersons { get; set; }
        public string Features { get; set; }
        public string Sponsor { get; set; }
        public string Specialconditions { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string Reason { get; set; }
    }
}
