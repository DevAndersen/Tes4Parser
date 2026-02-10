using System.Diagnostics;
using System.Text;
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
            int end = (int)Position;

            return Encoding.UTF8.GetString(span.Slice(start, end));
        }
    }
}
