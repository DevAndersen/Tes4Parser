namespace Tes4Parser;

public interface IReadWrite<TSelf>
{
    static abstract TSelf Read(Tes4Reader reader);

    static abstract void Write(Tes4Writer writer, TSelf record);
}
