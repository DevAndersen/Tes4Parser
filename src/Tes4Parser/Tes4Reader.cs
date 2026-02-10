using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tes4Parser.Records;

namespace Tes4Parser;

public sealed partial class Tes4Reader : IDisposable
{
    private readonly Stream _stream;
    private readonly BinaryReader _reader;

    private bool HasEndBeenReached => _stream.Position >= _stream.Length;

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

    public IEnumerable<Record> ReadRecords(bool isGroupLevel)
    {
        while (!HasEndBeenReached)
        {
            string typeString = ReadUtf8Value(4);

            // If at the group level, and the type string is all null bytes (seen with files that contains no groups), return.
            if (isGroupLevel && typeString == "\0\0\0\0")
            {
                yield break;
            }

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

    public void ReadTypeString(string typeString)
    {
        if (typeString.Length != 4)
        {
            throw new ArgumentException($"Type string '{typeString}' must be exactly four characters.", nameof(typeString));
        }

        string readTypeString = ReadUtf8Value(typeString.Length);
        if (readTypeString != typeString)
        {
            throw new InvalidDataException($"Read type string '{readTypeString}', expected '{typeString}'.");
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

    public FormId[] ReadFormList(string typeString)
    {
        ReadTypeString(typeString);
        ushort formIdCount = ReadU16Value();
        return MemoryMarshal.Cast<byte, FormId>(Read(formIdCount).Span).ToArray();
    }

    public FormId[] ReadFormListOptional(string typeString)
    {
        if (PeekdUtf8Value(typeString.Length) == typeString)
        {
            return ReadFormList(typeString);
        }

        return [];
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
