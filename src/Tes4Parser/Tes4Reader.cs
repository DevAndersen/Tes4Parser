using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tes4Parser.Records;

namespace Tes4Parser;

[DebuggerDisplay($"{{{nameof(_stream)}}}")]
public sealed partial class Tes4Reader : IDisposable
{
    private readonly Stream _stream;
    private readonly BinaryReader _reader;

    private bool HasEndBeenReached => _stream.Position >= _stream.Length;

    public long Position => _stream.Position;

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

    public Tes4Record ReadHeader()
    {
        ReadTypeString(Tes4Record.TypeString);
        return Tes4Record.Read(this);
    }

    public IEnumerable<Record> ReadRecords(bool isGroupLevel, uint? readLength = null)
    {
        uint? maxReadLength = (uint)_stream.Position + readLength;
        while (!HasEndBeenReached)
        {
            // If a read length has been specified, and that stream position has been reached, return.
            if (maxReadLength <= _stream.Position)
            {
                yield break;
            }

            string typeString = ReadUtf8Value(Tes4Constants.TypeStringLength);

            // If at the group level, and the type string is all null bytes (seen with files that contains no groups), return.
            if (isGroupLevel && typeString == "\0\0\0\0")
            {
                yield break;
            }

            Record? temporary = typeString switch
            {
                GroupRecord.TypeString => GroupRecord.Read(this),
                KeywordRecord.TypeString => KeywordRecord.Read(this),
                GlobalVariableRecord.TypeString => GlobalVariableRecord.Read(this),
                FactionRecord.TypeString => FactionRecord.Read(this),
                MagicEffectRecord.TypeString => MagicEffectRecord.Read(this),
                SpellRecord.TypeString => SpellRecord.Read(this),
                TextureSetRecord.TypeString => TextureSetRecord.Read(this),
                ProjectileRecord.TypeString => ProjectileRecord.Read(this),
                DialogTopicRecord.TypeString => DialogTopicRecord.Read(this),
                DialogTopicInfoRecord.TypeString => DialogTopicInfoRecord.Read(this),
                _ => null!
                //_ => throw new InvalidDataException($"Unexpected type string {typeString}")
            };

            if (temporary == null)
            {
                break;
            }

            yield return temporary;
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
        if (typeString.Length != Tes4Constants.TypeStringLength)
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
        if (typeString.Length != Tes4Constants.TypeStringLength)
        {
            throw new ArgumentException($"Type string '{typeString}' must be exactly four characters.", nameof(typeString));
        }

        return PeekdUtf8Value(typeString.Length) == typeString;
    }

    public T? ReadStructOptional<T>(string typeString) where T : struct
    {
        return PeekTypeString(typeString)
            ? ReadStruct<T>(typeString)
            : null;
    }

    public T ReadStruct<T>(string typeString) where T : struct
    {
        ReadTypeString(typeString);
        ushort size = _reader.ReadUInt16();
        ThrowIfNot(size, Unsafe.SizeOf<T>());

        return ReadStructValue<T>();
    }

    public T ReadStructValue<T>() where T : struct
    {
        ReadOnlyMemory<byte> structBytes = Read(Unsafe.SizeOf<T>());
        return MemoryMarshal.Read<T>(structBytes.Span);
    }

    public T[] ReadStructMultiple<T>(string typeString) where T : struct
    {
        ReadTypeString(typeString);

        ushort count = ReadU16Value(); // Todo: Implementation might not be correct, needs further testing.
        return MemoryMarshal.Cast<byte, T>(Read(count).Span).ToArray();
    }

    public IEnumerable<T> ReadMultipleFields<T>(string typeString) where T : struct
    {
        while (PeekTypeString(typeString))
        {
            ReadTypeString(typeString);

            ushort structSize = ThrowIfNot(ReadU16Value(), (ushort)Unsafe.SizeOf<T>());
            ReadOnlyMemory<byte> structBytes = Read(structSize);
            yield return MemoryMarshal.Read<T>(structBytes.Span);
        }
    }

    public FormId[] ReadFormList(string typeString)
    {
        ReadTypeString(typeString);
        ushort formIdCount = ReadU16Value();
        return MemoryMarshal.Cast<byte, FormId>(Read(formIdCount).Span).ToArray();
    }

    public FormId[] ReadFormListOptional(string typeString)
    {
        if (PeekTypeString(typeString))
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
