namespace Tes4Parser;

public partial class Tes4Reader
{
    public int ReadI32(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(int));
        return ReadI32Value();
    }

    public int? ReadI32Optional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadI32(typeString)
            : null;
    }

    public int ReadI32Value()
    {
        return _reader.ReadInt32();
    }
}
