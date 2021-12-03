using System;

namespace GarbageDBTest.Domain.Dtos.Request
{
    public class EventFilterRequest
    {
        public string Colony { get; set; }
        public string Reason { get; set; }
    }
}