namespace Tes4Parser;

public partial class Tes4Reader
{
    public byte ReadU8(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(byte));
        return ReadU8Value();
    }

    public byte? ReadU8Optional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadU8(typeString)
            : null;
    }

    public byte ReadU8Value()
    {
        return _reader.ReadByte();
    }
}
