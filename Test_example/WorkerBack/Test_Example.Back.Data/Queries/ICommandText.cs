

namespace Test_Example.Back.Data.Queries
{
    public interface ICommandText
    {
        string GetById { get; }
        string GetByParams { get; }
        string Add { get; }
    }
}
