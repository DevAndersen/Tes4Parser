using System.Text;

namespace Tes4Parser;

public partial class Tes4Reader
{
    public string ReadUtf8Value(int length)
    {
        ReadOnlyMemory<byte> stringBuffer = Read(length);
        return Encoding.UTF8.GetString(stringBuffer.Span);
    }

    public string PeekdUtf8Value(int length)
    {
        ReadOnlyMemory<byte> stringBuffer = Read(length);
        string typeString = Encoding.UTF8.GetString(stringBuffer.Span);
        _stream.Position -= length;
        return typeString;
    }

    public string ReadUtf8NullTerminated(string typeString)
    {
        ReadTypeString(typeString);
        return ReadUtf8NullTerminatedValue();
    }

    public string? ReadUtf8NullTerminatedOptional(string typeString)
    {
        return PeekTypeString(typeString)
            ? ReadUtf8NullTerminated(typeString)
            : null;
    }

    public string ReadUtf8NullTerminatedValue()
    {
        ushort length = _reader.ReadUInt16();
        ReadOnlyMemory<byte> stringBuffer = Read(length);

        if (stringBuffer.Span[^1] != '\0')
        {
            throw new InvalidDataException("Null-terminated string was not null-terminated");
        }

        return Encoding.UTF8.GetString(stringBuffer.Span[..^1]);
    }
}
