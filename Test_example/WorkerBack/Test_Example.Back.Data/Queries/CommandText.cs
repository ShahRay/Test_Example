

namespace Test_Example.Back.Data.Queries
{
    public class CommandText : ICommandText
    {
        public string GetById => "GetRequestById";

        public string GetByParams => "GetRequestsByParams";

        public string Add => "AddRequest";
    }
}
