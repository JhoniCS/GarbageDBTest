using System;

namespace GarbageDBTest.Domain.Dtos.Request
{
    public class POIFilterRequest
    {
        public DateTime? Date { get; set; }
        public string Colony { get; set; } //usuario
        public int Min { get; set; }
        public int Max { get; set; }
    }
}