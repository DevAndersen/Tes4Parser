using System.Text;

namespace Tes4Parser;

public partial class Tes4Writer
{
    public void WriteUtf8Value(string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        Write(bytes);
    }

    public void WriteUtf8(string typeString, string? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteU16Value((ushort)value.Length);
        WriteUtf8Value(value);
    }

    public void WriteUtf8NullTerminated(string typeString, string? value)
    {
        if (value == null)
        {
            return;
        }

        WriteTypeString(typeString);
        WriteUtf8NullTerminatedValue(value);
    }

    public void WriteUtf8NullTerminatedValue(string value)
    {
        WriteU16Value((ushort)(value.Length + 1));
        WriteUtf8Value(value);
        WriteU8Value(0);
    }
}
