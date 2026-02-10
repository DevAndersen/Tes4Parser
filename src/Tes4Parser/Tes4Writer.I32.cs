namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteI32(string typeString, int? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteI32Value(value.Value);
    }

    public void WriteI32Value(int value)
    {
        _writer.Write(value);
    }
}
