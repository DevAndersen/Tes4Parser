using System.Diagnostics;
using System.Text;

namespace Tes4Parser.Testing;

/// <summary>
/// An extension of <see cref="MemoryStream"/> which allows non-destructive peeking at the underlying buffer.
/// </summary>
[DebuggerDisplay($"{{{nameof(Debug)}}}")]
public class DebugWriteStream : MemoryStream
{
    public ReadOnlySpan<char> Debug
    {
        get
        {
            ReadOnlySpan<byte> span = GetBuffer().AsSpan();

            int start = int.Max((int)Position - 50, 0);
            int length = int.Min(50, (int)(Length - start));

            return span
                .Slice(start, length)
                .ToArray()
                .Select(x => (char)x)
                .ToArray();
        }
    }
}
