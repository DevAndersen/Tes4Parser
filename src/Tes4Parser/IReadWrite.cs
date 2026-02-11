namespace Tes4Parser;

public interface IReadWrite<TSelf> : IWrite
{
    static abstract TSelf Read(Tes4Reader reader);
}
