using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Tes4Parser;

#if DEBUG
[DebuggerDisplay($"{{{nameof(Debug)}}}")]
#endif
public class Tes4Reader : IDisposable
{
    private readonly Stream _stream;
    private readonly BinaryReader _reader;

#if DEBUG
    /// <summary>
    /// Preview of the upcoming bytes.
    /// </summary>
    private ReadOnlySpan<char> Debug => PeekdUtf8(64);
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

    public byte ReadU8(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(byte));
        return _reader.ReadByte();
    }

    public short ReadI16(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(short));
        return _reader.ReadInt16();
    }

    public ushort ReadU16(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(ushort));
        return _reader.ReadUInt16();
    }

    public int ReadI32(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(int));
        return _reader.ReadInt32();
    }

    public uint ReadU32(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(uint));
        return _reader.ReadUInt32();
    }

    public long ReadI64(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(long));
        return _reader.ReadInt64();
    }

    public ulong ReadU64(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(ulong));
        return _reader.ReadUInt64();
    }

    public float ReadF32(string typeString)
    {
        ReadTypeString(typeString);
        ThrowIfNot(_reader.ReadUInt16(), sizeof(float));
        return _reader.ReadSingle();
    }

    public string ReadUtf8(int length)
    {
        ReadOnlyMemory<byte> stringBuffer = Read(length);
        return Encoding.UTF8.GetString(stringBuffer.Span);
    }

    public string PeekdUtf8(int length)
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

        string readTypeString = ReadUtf8(typeString.Length);
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

        return PeekdUtf8(typeString.Length) == typeString;
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
