namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteI64(string typeString, long? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteI64Value(value.Value);
    }

    public void WriteI64Value(long value)
    {
        _writer.Write(value);
    }
}
