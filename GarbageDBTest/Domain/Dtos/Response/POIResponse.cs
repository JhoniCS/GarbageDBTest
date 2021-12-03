using System;

namespace GarbageDBTest.Domain.Dtos.Response
{
    public class POIResponse
    {
        public DateTime? Date { get; set; }
        public string Reason { get; set; } //usuario
        public string Description { get; set; } //usuario
        public int? Geoubication { get; set; }
        public string Colony { get; set; } //usuario
        public string Photo { get; set; } //usuario
        public int? Confirmations { get; set; }
        public int? Negations { get; set; }
    }
}