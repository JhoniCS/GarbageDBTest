using System;
using System.Collections.Generic;

#nullable disable

namespace GarbageDBTest.Domain.Entities

{
    public partial class Poi
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Reason { get; set; } //usuario
        public string Description { get; set; } //usuario
        public int? Geoubication { get; set; }
        public string Colony { get; set; } //usuario
        public string Photo { get; set; } //usuario
        public bool? IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Confirmations { get; set; }
        public int? Negations { get; set; }
        
    }
}
