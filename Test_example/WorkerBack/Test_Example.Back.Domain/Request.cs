using System;

namespace Test_Example.Back.Domain
{
    public class Request
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string DepartmentAddress { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string ClientIp { get; set; }
        public string RequestStatus { get; set; }
        public DateTime DateTime { get; set; }
    }
}
