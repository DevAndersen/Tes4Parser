namespace Tes4Parser;

public partial class Tes4Reader
{
    public short ReadI16(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(short));
        return ReadI16Value();
    }

    public short? ReadI16Optional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadI16(typeString)
            : null;
    }

    public short ReadI16Value()
    {
        return _reader.ReadInt16();
    }
}
