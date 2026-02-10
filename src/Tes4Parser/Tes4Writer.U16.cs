namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteU16(string typeString, ushort? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteU16Value(sizeof(ushort));
        WriteU16Value(value.Value);
    }

    public void WriteU16Value(ushort value)
    {
        _writer.Write(value);
    }
}
