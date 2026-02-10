namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteU8(string typeString, byte? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteU16Value(sizeof(byte));
        WriteU8Value(value.Value);
    }

    public void WriteU8Value(byte value)
    {
        _writer.Write(value);
    }
}
