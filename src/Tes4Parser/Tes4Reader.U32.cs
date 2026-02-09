namespace Tes4Parser;

public partial class Tes4Reader
{
    public uint ReadU32(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(uint));
        return ReadU32Value();
    }

    public uint? ReadU32Optional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadU32(typeString)
            : null;
    }

    public uint ReadU32Value()
    {
        return _reader.ReadUInt32();
    }
}
