namespace Tes4Parser;

public partial class Tes4Reader
{
    public ushort ReadU16(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(ushort));
        return ReadU16Value();
    }

    public ushort? ReadU16Optional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadU16(typeString)
            : null;
    }

    public ushort ReadU16Value()
    {
        return _reader.ReadUInt16();
    }
}
