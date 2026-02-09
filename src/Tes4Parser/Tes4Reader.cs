using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Tes4Parser.Records;

namespace Tes4Parser;

#if DEBUG
[DebuggerDisplay($"{{{nameof(Debug)}}}")]
#endif
public partial class Tes4Reader : IDisposable
{
    private readonly Stream _stream;
    private readonly BinaryReader _reader;

    private bool HasEndBeenReached => _stream.Position >= _stream.Length;

#if DEBUG
    /// <summary>
    /// Preview of the upcoming bytes.
    /// </summary>
    private ReadOnlySpan<char> Debug => PeekdUtf8Value(64);
#endif

    public Tes4Reader(Stream stream)
    {
        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream must be readable.", nameof(stream));
        }

        if (!stream.CanSeek)
        {
            throw new ArgumentException("Stream must be seekable.", nameof(stream));
        }

        _stream = stream;
        _reader = new BinaryReader(stream);
    }

    public IEnumerable<Record> ReadRecords()
    {
        while (!HasEndBeenReached)
        {
            string typeString = ReadUtf8Value(4);

            yield return typeString switch
            {
                Tes4Record.TypeString => Tes4Record.Read(this),
                //GroupRecord.TypeString => GroupRecord.Read(this),
                _ => throw new InvalidDataException($"Unexpected type string {typeString}")
            };
        }
    }

    public ReadOnlyMemory<byte> Read(int count)
    {
        return _reader.ReadBytes(count);
    }

    public ReadOnlyMemory<byte> Peek(int count)
    {
        ReadOnlyMemory<byte> bytes = _reader.ReadBytes(count);
        _stream.Position -= count;
        return bytes;
    }

    public ReadOnlyMemory<byte> Read(uint bytes)
    {
        return Read((int)bytes);
    }

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

    public string ReadUtf8NullTerminated()
    {
        ushort length = _reader.ReadUInt16();
        ReadOnlyMemory<byte> stringBuffer = Read(length);

        if (stringBuffer.Span[^1] != '\0')
        {
            throw new InvalidDataException("Null-terminated string was not null-terminated");
        }

        return Encoding.UTF8.GetString(stringBuffer.Span[..^1]);
    }

    public void ReadTypeString(string typeString)
    {
        if (typeString.Length != 4)
        {
            throw new ArgumentException($"Type string '{typeString}' must be exactly four characters.", nameof(typeString));
        }

        string readTypeString = ReadUtf8Value(typeString.Length);
        if (readTypeString != typeString)
        {
            throw new InvalidDataException($"Read type string '{typeString}', expected '{readTypeString}'.");
        }
    }

    public bool PeekTypeString(string typeString)
    {
        if (typeString.Length != 4)
        {
            throw new ArgumentException($"Type string '{typeString}' must be exactly four characters.", nameof(typeString));
        }

        return PeekdUtf8Value(typeString.Length) == typeString;
    }

    public T ReadStruct<T>(string typeString) where T : struct
    {
        ReadTypeString(typeString);

        ushort size = _reader.ReadUInt16();
        ThrowIfNot(size, Unsafe.SizeOf<T>());

        ReadOnlyMemory<byte> structBytes = Read(size);
        return MemoryMarshal.Read<T>(structBytes.Span);
    }

    private static T ThrowIfNot<T>(T actual, T expected)
    {
        if (actual == null || expected == null || !actual.Equals(expected))
        {
            throw new InvalidDataException($"Found '{actual}', expected '{expected}'.");
        }

        return actual;
    }

    public void Dispose()
    {
        _reader.Dispose();
    }
}
