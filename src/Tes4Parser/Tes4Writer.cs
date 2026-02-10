using System.Text;
using Tes4Parser.Records;

namespace Tes4Parser;

public sealed partial class Tes4Writer : IDisposable
{
    private readonly Stream _stream;
    private readonly BinaryWriter _writer;

    public Tes4Writer(Stream stream)
    {
        if (!stream.CanWrite)
        {
            throw new ArgumentException("Stream must be writeable.", nameof(stream));
        }

        if (!stream.CanSeek)
        {
            throw new ArgumentException("Stream must be seekable.", nameof(stream));
        }

        _stream = stream;
        _writer = new BinaryWriter(stream);
    }

    public void Write(Tes4Result result)
    {
        result.Header.Write(this);

        foreach (GroupRecord group in result.Groups)
        {
            //group.Write(this);
        }
    }

    public void Write(ReadOnlySpan<byte> bytes)
    {
        _stream.Write(bytes);
    }


    public void WriteTypeString(string typeString)
    {
        if (typeString.Length != 4)
        {
            throw new ArgumentException($"Type string '{typeString}' must be exactly four characters.", nameof(typeString));
        }

        WriteUtf8Value(typeString);
    }

    public void WriteUtf8Value(string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        Write(bytes);
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
