namespace Tes4Parser;

public partial class Tes4Reader
{
    public float ReadF32(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(float));
        return ReadF32Value();
    }

    public float? ReadF32Optional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadF32(typeString)
            : null;
    }

    public float ReadF32Value()
    {
        return _reader.ReadSingle();
    }
}
