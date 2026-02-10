namespace Tes4Parser;

public partial class Tes4Reader
{
    public FormId ReadFormId(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(uint));
        return ReadFormIdValue();
    }

    public FormId? ReadFormIdOptional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadFormId(typeString)
            : null;
    }

    public FormId ReadFormIdValue()
    {
        return (FormId)_reader.ReadUInt32();
    }
}
