namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteF32(string typeString, float? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteU16Value(sizeof(float));
        WriteF32Value(value.Value);
    }

    public void WriteF32Value(float value)
    {
        _writer.Write(value);
    }
}
