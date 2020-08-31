using System;

namespace Test_example.Domain
{
    public class ApiRequest
    {
        public Guid client_id { get; set; }
        public string department_address { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
    }
}
