using System.Diagnostics;
using System.Text;

/// <summary>
/// An extension of <see cref="MemoryStream"/> which allows non-destructive peeking at the underlying buffer.
/// </summary>
[DebuggerDisplay($"{{{nameof(Debug)}}}")]
public class DebugStream : MemoryStream
{
    public ReadOnlySpan<char> Debug
    {
        get
        {
            ReadOnlySpan<byte> span = GetBuffer().AsSpan();

            int start = (int)Position;
            int end = int.Min((int)Position + 50, (int)Length - (int)Position);

            return Encoding.UTF8.GetString(span.Slice(start, end));
        }
    }
}
