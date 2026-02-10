namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteFormId(string typeString, FormId? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteFormIdValue(value.Value);
    }

    public void WriteFormIdValue(FormId value)
    {
        _writer.Write(value.Value);
    }
}
