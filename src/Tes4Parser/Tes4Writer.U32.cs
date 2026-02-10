namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteU32(string typeString, uint? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteU16Value(sizeof(uint));
        WriteU32Value(value.Value);
    }

    public void WriteU32Value(uint value)
    {
        _writer.Write(value);
    }
}
