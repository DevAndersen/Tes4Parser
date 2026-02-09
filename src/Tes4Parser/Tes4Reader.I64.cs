namespace Tes4Parser;

public partial class Tes4Reader
{
    public long ReadI64(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(long));
        return ReadI64Value();
    }

    public long? ReadI64Optional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadI64(typeString)
            : null;
    }

    public long ReadI64Value()
    {
        return _reader.ReadInt64();
    }
}
