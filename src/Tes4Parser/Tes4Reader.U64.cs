namespace Tes4Parser;

public partial class Tes4Reader
{
    public ulong ReadU64(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(ulong));
        return ReadU64Value();
    }

    public ulong? ReadU64Optional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadU64(typeString)
            : null;
    }

    public ulong ReadU64Value()
    {
        return _reader.ReadUInt64();
    }
}
