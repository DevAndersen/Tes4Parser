using Tes4Parser.Records;

namespace Tes4Parser;

public sealed class Tes4Writer
{
    private readonly Stream _stream;

    public Tes4Writer(Stream stream)
    {
        _stream = stream;
    }

    public void Write(Tes4Result result)
    {
        result.Header.Write(this);

        foreach (GroupRecord group in result.Groups)
        {
            //group.Write(this);
        }
    }
}
