namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteI16(string typeString, short? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteI16Value(value.Value);
    }

    public void WriteI16Value(short value)
    {
        _writer.Write(value);
    }
}
