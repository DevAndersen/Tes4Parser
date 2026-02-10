namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteU64(string typeString, ulong? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteU16Value(sizeof(ulong));
        WriteU64Value(value.Value);
    }

    public void WriteU64Value(ulong value)
    {
        _writer.Write(value);
    }
}
